import * as React from 'react';

export class Layout extends React.Component {

  public render() {
    return (
      <main role="main" className="container">
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
          <div className="container">
            <a className="navbar-brand" href="#">Cars</a>
          </div>
        </nav>
        {this.props.children}
      </main>
    );
  }
}
