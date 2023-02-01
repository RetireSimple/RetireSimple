import React from 'react';
import ReactDOM from 'react-dom/client';
import {
	createBrowserRouter,
	createRoutesFromElements,
	Route,
	RouterProvider,
} from 'react-router-dom';
import {getInvestment, getInvestments} from './api/InvestmentApi';
import {Layout} from './App';
import {AddInvestmentDialog} from './components/dialogs/AddInvestmentDialog';
import {flattenApiInvestment} from './data/ApiMapper';
import './index.css';
import {InvestmentView} from './routes/InvestmentView';
import {Root} from './routes/Root';

const root = ReactDOM.createRoot(document.getElementById('root'));

const router = createBrowserRouter(
	createRoutesFromElements([
		<Route path='/' element={<Layout />} loader={async () => await getInvestments()}>
			<Route path='/' element={<Root />} />
			<Route
				path='investment/:id'
				element={<InvestmentView />}
				loader={async ({params}) => flattenApiInvestment(await getInvestment(params.id))}
			/>
		</Route>,
		<Route path='investment/add' element={<AddInvestmentDialog />} />,
	]),
);

root.render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>,
);
