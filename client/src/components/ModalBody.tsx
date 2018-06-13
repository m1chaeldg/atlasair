import * as React from 'react';

export class ModalBody extends React.Component{
    public render() {
        return (
            <div className="modal-body">
                {this.props.children}
            </div>
        );
    }
}