import React from 'react';
import ReactDOM from 'react-dom/client';
import {
	createBrowserRouter,
	createRoutesFromElements,
	Route,
	RouterProvider,
} from 'react-router-dom';
import {addStock, getInvestment, getInvestments} from './api/InvestmentApi';
import {Layout} from './Layout';
import {AddInvestmentDialog} from './components/dialogs/AddInvestmentDialog';
import {flattenApiInvestment} from './data/ApiMapper';
import './index.css';
import {InvestmentView} from './routes/InvestmentView';
import {Root} from './routes/Root';

const root = ReactDOM.createRoot(document.getElementById('root'));

const addInvestmentAction = async ({params, request}) => {
	const requestData = await request.json();
	return addStock(requestData);
};

const router = createBrowserRouter(
	createRoutesFromElements([
		<Route path='/' element={<Layout />} loader={async () => await getInvestments()}>
			<Route path='/' element={<Root />} />
			<Route
				path='investment/:id'
				element={<InvestmentView />}
				loader={async ({params}) => flattenApiInvestment(await getInvestment(params.id))}>
				<Route path='add' element={<AddInvestmentDialog />} />
			</Route>
			<Route path='add' action={addInvestmentAction} element={<AddInvestmentDialog />} />,
		</Route>,
	]),
);

root.render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>,
);
