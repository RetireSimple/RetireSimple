import {Box, CircularProgress, Icon, Link, Typography} from '@mui/material';
import {SnackbarProvider} from 'notistack';
import React from 'react';
import ReactDOM from 'react-dom/client';
import {
	Route,
	RouterProvider,
	createBrowserRouter,
	createRoutesFromElements,
	isRouteErrorResponse,
	useRouteError,
} from 'react-router-dom';
import {Layout} from './Layout';
import {getPortfolio} from './api/ApiCommon';
import {flattenApiInvestment, getFlatVehicleData} from './api/ApiMapper';
import {convertFromDecimal} from './api/ConvertUtils';
import {deleteInvestment, getInvestment} from './api/InvestmentApi';
import {deleteVehicle, getVehicle} from './api/VehicleApi';
import {InvestmentsPage} from './pages/InvestmentsPage';
import {VehiclesPage} from './pages/VehiclesPage';
import {AboutPage} from './pages/AboutPage';
import './index.css';
import { HelpPage } from './pages/HelpPage';
import { ExpensesPage } from './pages/ExpensesPage';

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

/**********************************
 * Error Page
 *********************************/
const ErrorPage = () => {
	const error = useRouteError();

	return (
		<Box
			sx={{
				display: 'flex',
				justifyContent: 'space-around',
				alignItems: 'center',
				flexDirection: 'column',
			}}>
			<Box sx={{display: 'flex', flexDirection: 'row'}}>
				<Icon
					baseClassName='material-icons'
					color='error'
					sx={{fontSize: '4rem', marginRight: '1rem', padding: '0.25rem'}}>
					error
				</Icon>
				<Typography variant='h2'>Oops...</Typography>
			</Box>
			<br />
			<Typography variant='h4'>Something went wrong and RetireSimple crashed.</Typography>
			<br />
			<Typography variant='h5'>
				Try refreshing this page, or restarting the <code>RetireSimple.Backend</code>{' '}
				application.
			</Typography>
			<br />
			<Typography variant='h5'>
				If the problem persists, please report a bug{' '}
				<Link href='https://github.com/RetireSimple/RetireSimple/issues/new/choose'>
					on the GitHub Issues page
				</Link>
				.
			</Typography>
			{isRouteErrorResponse(error) && (
				<Typography variant='h1' component='div' sx={{textAlign: 'center'}}>
					{error.status}
				</Typography>
			)}
		</Box>
	);
};

/************************
 * React Router Route Defs.
 ************************/
const router = createBrowserRouter(
	createRoutesFromElements([
		<Route
			path='/'
			element={<Layout />}
			errorElement={<ErrorPage />}
			id='root'
			loader={async () => await getPortfolio()}>
			<Route
				path='/'
				element={
					<SuspenseRoute>
						<RootView />
					</SuspenseRoute>
				}
			/>
			
			<Route
				path='InvestmentPage/'
				//why are we loading in the entire portfolio???
				loader={async () => await getPortfolio()}
				element={
					<InvestmentsPage />
				}
			/>
			<Route
				path='VehiclesPage/'
				//why are we loading in the entire portfolio???
				loader={async () => await getPortfolio()}
				element={
					<VehiclesPage />
				}
			/>
			<Route
				path='ExpensesPage/'
				element={
					<ExpensesPage />
				}
			/>
			<Route
				path='AboutPage/'
				element={
					<AboutPage />
				}
			/>
			<Route
				path='HelpPage/'
				element={
					<HelpPage />
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

const fallback = (
	<Box>
		<Typography variant='h1' component='div' sx={{textAlign: 'center'}}>
			Loading...
		</Typography>
	</Box>
);

root.render(
	<React.StrictMode>
		<SnackbarProvider>
			<RouterProvider router={router} fallbackElement={fallback} />
		</SnackbarProvider>
	</React.StrictMode>,
);
