import * as React from 'react';


// tslint:disable-next-line:interface-name
export interface ModalHeaderProp {
    header: string;
}

export class ModalHeader extends React.Component<ModalHeaderProp, {}>{
    constructor(prop: ModalHeaderProp) {
        super(prop);
    }

    public render() {
        return (
            <div className="modal-header">
                <h5 className="modal-title" id="exampleModalLabel">{this.props.header}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        );
    }
}