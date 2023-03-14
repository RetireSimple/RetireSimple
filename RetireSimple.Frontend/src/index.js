import React from 'react';
import ReactDOM from 'react-dom/client';
import {
	createBrowserRouter,
	createRoutesFromElements,
	Route,
	RouterProvider,
} from 'react-router-dom';
import { getPortfolio } from './api/ApiCommon';
import { deleteInvestment, getInvestment } from './api/InvestmentApi';
import { flattenApiInvestment, getFlatVehicleData } from './data/ApiMapper';
import './index.css';
import { Layout } from './Layout';
import { InvestmentView } from './routes/InvestmentView';
import { Root } from './routes/Root';
import { VehicleView } from './routes/VehicleView';
import { deleteVehicle, getVehicle } from './api/VehicleApi';

const root = ReactDOM.createRoot(document.getElementById('root'));

const addInvestmentAction = async () => {
	//Assert that the actual addition occured already
	//Use this as a way to refresh loader data
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const updateInvestmentAction = async ({params, request}) => {
	return new Response(null, {status: 302, headers: {Location: `/investment/${params.id}`}});
};

const deleteInvestmentAction = async ({params}) => {
	const id = params.id;
	await deleteInvestment(id);
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const addVehicleAction = async () => {
	//Assert that the actual addition occured already
	//Use this as a way to refresh loader data
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const updateVehicleAction = async ({params, request}) => {
	return new Response(null, {status: 302, headers: {Location: `/vehicle/${params.id}`}});
};

const deleteVehicleAction = async ({params}) => {
	const id = params.id;
	await deleteVehicle(id);
	return new Response(null, {status: 302, headers: {Location: '/'}});
}

const router = createBrowserRouter(
	createRoutesFromElements([
		<Route path='/' element={<Layout />} id='root'
			loader={async () => await getPortfolio()}>
			<Route path='/' element={<Root />} />
			<Route
				path='investment/:id'
				element={<InvestmentView />}
				loader={async ({params}) => flattenApiInvestment(await getInvestment(params.id))}>
				<Route path='update' action={updateInvestmentAction} />
				<Route path='delete' action={deleteInvestmentAction} />
			</Route>
			<Route
				path='vehicle/:id'
				element={<VehicleView />}
				loader={async({params}) => getFlatVehicleData(await getVehicle(params.id))}>
				<Route path='update' action={updateVehicleAction} />
				<Route path='delete' action={deleteVehicleAction} />
			</Route>
			<Route path='addVehicle' action={addVehicleAction} />
			<Route path='add' action={addInvestmentAction} />
			<Route path='*' element={<div>404</div>} />
		</Route>,
	]),
);

root.render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>,
);
