import React from 'react';
import ReactDOM from 'react-dom/client';
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import './index.css';
import {Root} from './routes/Root';
import {InvestmentView} from './routes/InvestmentView';

const router = createBrowserRouter([
	{
		path: '/',
		element: <Root />,
		children: [
			{path: 'investment/:id', element: <InvestmentView />},
			// {path: '*', element: <Root />},
		],
	},
]);

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>,
);
