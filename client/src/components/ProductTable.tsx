import * as React from "react";
import { ProductItem } from "../ProductService";


// tslint:disable-next-line:interface-name
interface Prop {
    Products: ProductItem[];
    onEdit: (productId: string) => void;
    onDelete: (productId: string) => void;
    onView: (productId: string) => void;

    viewModalId: string;
    editModalId: string;
    deleteModalId: string;
}

export class ProductTable extends React.Component<Prop, {}> {

    public render() {


        return (
            <table className="table table-striped table-hover">
                <caption>List of products</caption>
                <thead>
                    <tr>
                        <th scope="col">Manufacturer</th>
                        <th scope="col">Make</th>
                        <th scope="col">Model</th>
                        <th scope="col">Year</th>
                        <th scope="col" />
                    </tr>
                </thead>
                <tbody>
                    {this.props.Products.map((product, i) =>
                        <tr key={i}>
                            <td>
                                {this.renderViewButton(product.Manufacturer, product.Id)}
                            </td>
                            <td>{product.Make} </td>
                            <td>{product.Model} </td>
                            <td>{product.Year} </td>
                            <td>
                                {this.renderEditButton(product.Id)}
                                &nbsp;
                                {this.renderDeleteButton(product.Id)}
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>);
    }

    private renderViewButton(label: string, productId: string, ) {
        return (<a href="#"
            data-toggle="modal" data-target={this.props.viewModalId}
            // tslint:disable-next-line:jsx-no-lambda
            onClick={() => { this.viewClick(productId); }}
        >
            {label}
        </a>);
    }

    private renderEditButton(productId: string) {
        return (<a href="#"
            data-toggle="modal" data-target={this.props.editModalId}
            // tslint:disable-next-line:jsx-no-lambda
            onClick={() => { this.editClick(productId); }}
        >
            <i className="fas fa-pencil-alt" />
        </a>);
    }
    private renderDeleteButton(productId: string) {
        return (<a href="#"
            data-toggle="modal" data-target={this.props.deleteModalId}
            // tslint:disable-next-line:jsx-no-lambda
            onClick={() => { this.deleteClick(productId); }}
        >
            <i className="fas fa-trash-alt" />
        </a>);
    }
    private viewClick(productId: string): void {
        this.props.onView(productId);
    }
    private editClick(productId: string): void {
        this.props.onEdit(productId);
    }

    private deleteClick(productId: string): void {
        this.props.onDelete(productId);
    }
}