import React from 'react';
import ReactDOM from 'react-dom/client';
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import {getInvestments, getInvestment} from './api/InvestmentApi';
import './index.css';
import {InvestmentView} from './routes/InvestmentView';
import {Root} from './routes/Root';

const router = createBrowserRouter([
	{
		path: '/',
		element: <Root />,
		loader: async () => await getInvestments(),
		children: [
			{
				path: 'investment/:id',
				element: <InvestmentView />,
				loader: async ({params}) => await getInvestment(params.id),
			},
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
