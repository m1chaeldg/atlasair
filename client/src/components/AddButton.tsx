import * as React from "react";

// tslint:disable-next-line:interface-name
export interface AddButtonProp {
    targetModelId: string;
}

export class AddButton extends React.Component<AddButtonProp, {}>{
    constructor(prop: AddButtonProp) {
        super(prop);
    }

    public render() {
        const targetId = this.props.targetModelId;
        return (
            <button type="button" className="btn btn-primary float-right" data-target={targetId} data-toggle="modal" >
                Add
            </button>
        );
    }
}