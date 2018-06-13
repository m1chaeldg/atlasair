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
}
// tslint:disable-next-line:interface-name
interface State {
    Manufacturer: string;
    Make: string;
    Model: string;
    Year: string;
}

export class ProductView extends React.Component<Prop, State>{

    constructor(prop: Prop) {
        super(prop);

        this.state = {
            Make: '',
            Manufacturer: '',
            Model: '',
            Year: '',
        }
    }
    public componentWillReceiveProps?(nextProps: Readonly<Prop>, nextContext: any): void {
        this.loadDetails(nextProps.productId);
    }
    public componentWillMount() {
        this.loadDetails(this.props.productId);
    }
    public render() {

        const textInput = (label: string, value: string) => {
            return (
                <div className="form-group row">
                    <label
                        className="col-sm-3 col-form-label">{label}</label>
                    <div className="col-sm-9">
                        <input type="text"
                            readOnly={true}
                            className="form-control"
                            value={value} />
                    </div>
                </div>);
        }

        return (

            <form role="form">
                <Modal modalId={this.props.modalId}>
                    <ModalHeader header="Car Details" />
                    <ModalBody>
                        {textInput("Manufacturer", this.state.Manufacturer)}
                        {textInput("Make", this.state.Make)}
                        {textInput("Model", this.state.Model)}
                        {textInput("Year", this.state.Year)}
                    </ModalBody>
                    <ModalFooter>
                        <button type="button" className="btn btn-secondary"
                            data-dismiss="modal">
                            Close
                        </button>
                    </ModalFooter>
                </Modal>
            </form>
        );
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
                    Year: data.Year
                })
            });

    }
}