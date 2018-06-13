import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.css';
import registerServiceWorker from './registerServiceWorker';

let baseUrl: string | null = "";
const baseElement = document.getElementsByTagName('base');
if (baseElement !== null) {
  baseUrl = baseElement[0].getAttribute('href');
}

const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl || ""}>
    <App />
  </BrowserRouter>,
  rootElement
);
registerServiceWorker();
