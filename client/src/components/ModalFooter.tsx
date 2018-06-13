import * as React from 'react';

export class ModalFooter extends React.Component {
    public render() {
        return (
            <div className="modal-footer">
                {this.props.children}
            </div>
        );
    }
}