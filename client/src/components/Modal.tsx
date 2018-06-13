import * as React from 'react';

// tslint:disable-next-line:interface-name
export interface ModalProp {
    modalId: string;
}
export class Modal extends React.Component<ModalProp, {}> {

    constructor(prop: ModalProp) {
        super(prop);
    }
    
    public render() {
        return (
            <div className="modal fade" id={this.props.modalId} role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div className="modal-dialog modal-dialog-centered" role="document">
                    <div className="modal-content">
                        {this.props.children}
                    </div>
                </div>
            </div>
        );
    }
}

