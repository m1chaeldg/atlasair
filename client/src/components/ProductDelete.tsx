import * as React from 'react';
import { ProductService } from '../ProductService';
import { Modal } from './Modal';
import { ModalBody } from './ModalBody';
import { ModalFooter } from './ModalFooter';
import { ModalHeader } from './ModalHeader';


// tslint:disable-next-line:interface-name
interface Prop {
    modalId: string;
    productId: string;
    successCreateCallback: () => void;
}
// tslint:disable-next-line:interface-name
interface State {
    Id: string;
    Make: string;
    Manufacturer: string;
    Model: string;
    Year: string;
    saving: boolean;
}

export class ProductDelete extends React.Component<Prop, State>{
    private btnClose: HTMLButtonElement | null;

    constructor(prop: Prop) {
        super(prop);
        this.state = {
            Id: prop.productId,
            Make: '',
            Manufacturer: '',
            Model: '',
            Year: '',
            saving: false,
        }
    }
    public componentWillReceiveProps?(nextProps: Readonly<Prop>, nextContext: any): void {
        if (nextProps.productId !== this.state.Id) {
            this.loadDetails(nextProps.productId);
            this.setState({
                Id: nextProps.productId
            });
        }
    }
    public componentWillMount() {
        this.loadDetails(this.state.Id);
    }
    public render() {

        return (

            <form role="form">
                <Modal modalId={this.props.modalId}>
                    <ModalHeader header="Delete Car" />
                    <ModalBody>
                        <p>Are you sure you want to delete the:
                        </p>
                        <p>
                            {this.state.Year}
                            &nbsp;
                            {this.state.Manufacturer}
                            &nbsp;
                            {this.state.Make}
                            &nbsp;
                            {this.state.Model}
                        </p>
                    </ModalBody>
                    <ModalFooter>
                        <button type="button" className="btn btn-primary"
                            disabled={this.state.saving}
                            // tslint:disable-next-line:jsx-no-lambda
                            onClick={(e) => { this.onSaveClik(e); }}
                        >
                            Save
                        </button>
                        <button type="button" className="btn btn-secondary"
                            data-dismiss="modal"
                            ref={input => this.btnClose = input}>
                            Close
                        </button>
                    </ModalFooter>
                </Modal>
            </form>
        );
    }

    private closeModal(): void {
        if (this.btnClose) {
            this.btnClose.click();
        }
    }

    private onSaveClik(event: React.MouseEvent<HTMLButtonElement>): void {
        event.preventDefault();
        this.setState({
            saving: true
        });
        const service = new ProductService();
        service.delete(this.state.Id)
            .then(() => {
                this.setState({
                    Id: "",
                    Make: "",
                    Manufacturer: "",
                    Model: "",
                    Year: "",
                    saving: false
                });

                this.closeModal();
                this.props.successCreateCallback();
            })


    }

    private loadDetails(productId: string) {
        if (!productId || productId === "") {
            return;
        }

        const service = new ProductService();
        service.getById(productId)
            .then(data => {
                this.setState({
                    Make: data.Make,
                    Manufacturer: data.Manufacturer,
                    Model: data.Model,
                    Year: data.Year,
                    saving: false
                })
            });

    }
}