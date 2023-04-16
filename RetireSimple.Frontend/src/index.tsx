import React from 'react';
import ReactDOM from 'react-dom/client';
import {
	createBrowserRouter,
	createRoutesFromElements,
	Route,
	RouterProvider,
} from 'react-router-dom';
import {getPortfolio} from './api/ApiCommon';
import {flattenApiInvestment, getFlatVehicleData} from './api/ApiMapper';
import {deleteInvestment, getInvestment} from './api/InvestmentApi';
import {deleteVehicle, getVehicle} from './api/VehicleApi';
import './index.css';
import {Layout} from './Layout';
import {CircularProgress, Typography} from '@mui/material';
import {convertFromDecimal} from './api/ConvertUtils';
import {ApiPresetData} from './Interfaces';

/************************
 * Lazy Loaded Components
 ***********************/

const InvestmentView = React.lazy(() =>
	import('./routes/InvestmentView').then((module) => ({default: module.InvestmentView})),
);

const RootView = React.lazy(() =>
	import('./routes/Root').then((module) => ({default: module.Root})),
);
const VehicleView = React.lazy(() =>
	import('./routes/VehicleView').then((module) => ({default: module.VehicleView})),
);

/************************
 * Suspense Wrapper
 ***********************/
export const SuspenseRoute = ({children}: {children: React.ReactNode}) => {
	return <React.Suspense fallback={<CircularProgress />}>{children}</React.Suspense>;
};

/************************
 * Preset Context
 ************************/
export const PresetConext = React.createContext<ApiPresetData>({});
export const PresetProvider = PresetConext.Provider;

/************************
 * React Router Actions
 ************************/
const addInvestmentAction = async () => {
	//Assert that the actual addition occured already
	//Use this as a way to refresh loader data
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const updateInvestmentAction = async ({params}: {params: any}) => {
	return new Response(null, {status: 302, headers: {Location: `/investment/${params.id}`}});
};

const deleteInvestmentAction = async ({params}: {params: any}) => {
	const id = params.id;
	await deleteInvestment(id);
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const addVehicleAction = async () => {
	//Assert that the actual addition occured already
	//Use this as a way to refresh loader data
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

const updateVehicleAction = async ({params}: {params: any}) => {
	return new Response(null, {status: 302, headers: {Location: `/vehicle/${params.id}`}});
};

const deleteVehicleAction = async ({params}: {params: any}) => {
	const id = params.id;
	await deleteVehicle(id);
	return new Response(null, {status: 302, headers: {Location: '/'}});
};

/************************
 * React Router Route Defs.
 ************************/
const router = createBrowserRouter(
	createRoutesFromElements([
		<Route path='/' element={<Layout />} id='root' loader={async () => await getPortfolio()}>
			<Route
				path='/'
				element={
					<SuspenseRoute>
						<RootView />
					</SuspenseRoute>
				}
			/>
			<Route
				path='investment/:id'
				element={
					<SuspenseRoute>
						<InvestmentView />
					</SuspenseRoute>
				}
				loader={async ({params}) => {
					const data = flattenApiInvestment(
						await getInvestment(parseInt(params.id ?? '')),
					);
					convertFromDecimal(data);
					return data;
				}}>
				<Route path='update' action={updateInvestmentAction} />
				<Route path='delete' action={deleteInvestmentAction} />
			</Route>
			<Route
				path='vehicle/:id'
				element={
					<SuspenseRoute>
						<VehicleView />
					</SuspenseRoute>
				}
				loader={async ({params}) => {
					const data = getFlatVehicleData(await getVehicle(parseInt(params.id ?? '')));
					convertFromDecimal(data);
					return data;
				}}>
				<Route path='update' action={updateVehicleAction} />
				<Route path='delete' action={deleteVehicleAction} />
			</Route>
			<Route path='addVehicle' action={addVehicleAction} />
			<Route path='add' action={addInvestmentAction} />
			<Route path='*' element={<div>404</div>} />
		</Route>,
	]),
);

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement);

root.render(
	<React.StrictMode>
		<RouterProvider router={router} fallbackElement={<Typography>Testing</Typography>} />
	</React.StrictMode>,
);
