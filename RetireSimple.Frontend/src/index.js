import React from 'react';
import ReactDOM from 'react-dom/client';
import {
	createBrowserRouter,
	createRoutesFromElements,
	Route,
	RouterProvider,
} from 'react-router-dom';
import {deleteInvestment, getInvestment, getInvestments} from './api/InvestmentApi';
import {flattenApiInvestment} from './data/ApiMapper';
import './index.css';
import {Layout} from './Layout';
import {InvestmentView} from './routes/InvestmentView';
import {Root} from './routes/Root';

const root = ReactDOM.createRoot(document.getElementById('root'));

const addInvestmentAction = async () => {
	//Assert that the actual addition occured already
	//Use this as a way to refresh loader data
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const updateInvestmentAction = async ({params, request}) => {
	// const requestData = await request.json();
	// return addStock(requestData);
};

const deleteInvestmentAction = async ({params}) => {
	const id = params.id;
	await deleteInvestment(id);
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const router = createBrowserRouter(
	createRoutesFromElements([
		<Route path='/' element={<Layout />} id='root' loader={async () => await getInvestments()}>
			<Route path='/' element={<Root />} />
			<Route
				path='investment/:id'
				element={<InvestmentView />}
				loader={async ({params}) => flattenApiInvestment(await getInvestment(params.id))}>
				<Route path='update' action={updateInvestmentAction} />
				<Route path='delete' action={deleteInvestmentAction} />
			</Route>
			<Route path='add' action={addInvestmentAction} />,
		</Route>,
	]),
);

root.render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>,
);
