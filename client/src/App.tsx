import * as React from 'react';
import { Route } from 'react-router';
// import { Home } from './components/Home';
import { Layout } from './components/Layout';
// import { NotFound } from './components/NotFound';
import { ProductList } from './components/ProductList';


export default class App extends React.Component {
  public displayName = App.name
  public render() {
    return (
      <Layout>
        <Route path='/' component={ProductList} />
      </Layout>
    );
  }
}