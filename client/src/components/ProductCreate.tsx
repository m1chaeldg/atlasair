import * as React from 'react';
import { ProductService } from '../ProductService';
import { Modal } from './Modal';
import { ModalBody } from './ModalBody';
import { ModalFooter } from './ModalFooter';
import { ModalHeader } from './ModalHeader';


// tslint:disable-next-line:interface-name
interface Prop {
    successCreateCallback: () => void;
    modalId: string;
}
// tslint:disable-next-line:interface-name
interface State {
    Manufacturer: string;
    Make: string;
    Model: string;
    Year: string;
    saving: boolean;
}

export class ProductCreate extends React.Component<Prop, State>{
    private btnClose: HTMLButtonElement | null;
    private btnSave: HTMLButtonElement | null;

    constructor(prop: Prop) {
        super(prop)
        this.state = {
            Make: '',
            Manufacturer: '',
            Model: '',
            Year: '',
            saving: false
        }
    }

    public render() {

        const textInput = (elId: string, label: string, value: string) => {
            return (
                <div className="form-group row">
                    <label htmlFor={elId}
                        className="col-sm-3 col-form-label">{label}</label>
                    <div className="col-sm-9">
                        <input type="text"
                            className="form-control"
                            id={elId}
                            value={value}
                            // tslint:disable-next-line:jsx-no-lambda
                            onChange={(e) => this.handleChange(label, e.target.value)}
                        />
                    </div>
                </div>);
        }
        return (

            <form role="form">
                <Modal modalId={this.props.modalId}>
                    <ModalHeader header="Add New Car" />
                    <ModalBody>
                        {textInput("manufacturer", "Manufacturer", this.state.Manufacturer)}
                        {textInput("make", "Make", this.state.Make)}
                        {textInput("model", "Model", this.state.Model)}
                        {textInput("year", "Year", this.state.Year)}
                    </ModalBody>
                    <ModalFooter>
                        <button type="button" className="btn btn-primary"
                            disabled={this.state.saving}
                            // tslint:disable-next-line:jsx-no-lambda
                            onClick={(e) => { this.onSaveClik(e); }}
                            ref={input => this.btnSave = input}>
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
    private handleChange(model: string, value: string): void {
        const dummy = {};
        dummy[model] = value;

        this.setState(dummy);
    }
    private onSaveClik(event: React.MouseEvent<HTMLButtonElement>): void {
        event.preventDefault();
        this.setState({
            saving: true
        });
        const service = new ProductService();
        service.create({
            Make: this.state.Make,
            Manufacturer: this.state.Manufacturer,
            Model: this.state.Model,
            Year: this.state.Year,
        }).then(() => {
            this.setState({
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
}