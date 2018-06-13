import * as React from 'react';
import { ProductItem, ProductService } from '../ProductService';
import { AddButton } from "./AddButton";
import { ProductCreate } from './ProductCreate';
import { ProductDelete } from './ProductDelete';
import { ProductEdit } from './ProductEdit';
import { ProductTable } from "./ProductTable";
import { ProductView } from './ProductView';

// tslint:disable-next-line:interface-name
interface State {
  loading: boolean;
  products: ProductItem[];
  productIdToView: string;
  productIdToEdit: string;
  productIdToDelete: string;
}

export class ProductList extends React.Component<{}, State> {

  private productService: ProductService;

  constructor(prop: {}) {
    super(prop);

    this.productService = new ProductService();
    this.state = {
      loading: true,
      productIdToDelete: "",
      productIdToEdit: "",
      productIdToView: "",
      products: [],
    }
  }


  public componentWillMount() {
    this.loadProducts();
  }

  public render() {

    const contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <ProductTable
        Products={this.state.products}
        // tslint:disable-next-line:jsx-no-lambda
        onDelete={(id) => { this.delete(id); }}
        // tslint:disable-next-line:jsx-no-lambda
        onEdit={(id) => { this.edit(id); }}
        // tslint:disable-next-line:jsx-no-lambda
        onView={(id) => { this.view(id); }}
        viewModalId="#viewmodal"
        editModalId="#editmodal"
        deleteModalId="#deletemodal"
      />;

    return (
      <div className="container-fluid">
        <div className="row">

          <div className="col-sm-12">
            <AddButton targetModelId="#addmodal" />
            {contents}
          </div>

          <ProductCreate modalId="addmodal"
            // tslint:disable-next-line:jsx-no-lambda
            successCreateCallback={() => { this.successCreateCallback() }} />

          <ProductView modalId="viewmodal" productId={this.state.productIdToView} />

          <ProductEdit modalId="editmodal" productId={this.state.productIdToEdit}
            // tslint:disable-next-line:jsx-no-lambda
            successCreateCallback={() => { this.successCreateCallback() }} />

          <ProductDelete modalId="deletemodal" productId={this.state.productIdToDelete}
            // tslint:disable-next-line:jsx-no-lambda
            successCreateCallback={() => { this.successCreateCallback() }} />
        </div>
      </div>
    );
  }

  private edit(productId: string): void {
    this.setState({
      productIdToEdit: productId,
    });
  }

  private view(productId: string): void {
    this.setState({
      productIdToView: productId
    });
  }

  private delete(productId: string): void {
    this.setState({
      productIdToDelete: productId
    });
  }
  private successCreateCallback() : void{
    this.setState({
      productIdToDelete: "",
      productIdToEdit: "",
      productIdToView: "",
      
    });

    this.loadProducts();
  }
  private loadProducts() {
    this.productService.getAll()
      .then(products => {
        this.setState(
          {
            "loading": false,
            "products": products
          }
        )
      })
  }
}