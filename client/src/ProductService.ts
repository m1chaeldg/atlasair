// tslint:disable-next-line:interface-name
// tslint:disable-next-line:no-empty-interface
// tslint:disable-next-line:interface-name
export interface ProductItem extends CreateProductItem {
    Id: string;
}

// tslint:disable-next-line:interface-name
export interface CreateProductItem {
    Manufacturer: string;
    Make: string;
    Model: string;
    Year: string;
}

export class ProductService {
    private apiUrl: string = "http://localhost:5000";

    /**
     * get all products
     */
    public getAll(): Promise<ProductItem[]> {
        return fetch(`${this.apiUrl}/api/products`)
            .then(response => response.json());
    }

    /**
     * get product by product id
     * @param id product id
     */
    public getById(id: string): Promise<ProductItem> {
        return fetch(`${this.apiUrl}/api/products/${id}`)
            .then(response => response.json());
    }

    /**
     * Create a product
     * @param product product details to create
     */
    public create(product: CreateProductItem): Promise<ProductItem> {
        return fetch(`${this.apiUrl}/api/products`, {
            body: JSON.stringify(product),
            headers: {
                "Content-Type": "application/json"
            },
            method: "post",

        })
            .then(response => response.json());
    }

    /**
     * Update product
     * @param product product details to update
     */
    public update(product: ProductItem): Promise<boolean> {
        return fetch(`${this.apiUrl}/api/products/${product.Id}`, {
            body: JSON.stringify(product),
            headers: {
                "Content-Type": "application/json"
            },
            method: "put"
        })
            .then(response => response.status === 200);
    }
    /**
     * Delete product
     * @param id product id to delete
     */
    public delete(id: string): Promise<boolean> {
        return fetch(`${this.apiUrl}/api/products/${id}`, {
            method: "delete"
        }).then(response => response.status === 200);
    }
}