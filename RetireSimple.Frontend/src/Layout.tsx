import {
	AppBar,
	Box,
	Divider,
	Icon,
	IconButton,
	List,
	ListSubheader,
	MenuItem,
	Paper,
	Tooltip,
	Typography,
} from '@mui/material';
import React from 'react';
import {Link, Outlet, useLoaderData} from 'react-router-dom';
import {SidebarInvestment} from './components/Sidebar/InvestmentListItem';
import {VehicleListItem} from './components/Sidebar/VehicleListItem';
import {AddInvestmentDialog} from './components/dialogs/AddInvestmentDialog';
import {Investment, Portfolio} from './data/Interfaces';

export const Layout = () => {
	const portfolio = useLoaderData() as Portfolio;
	const {investments, investmentVehicles: vehicles} = portfolio;

	const [addDialogOpen, setAddDialogOpen] = React.useState(false);

	const renderInvestmentsTable = (investments: Investment[]) => {
		return (
			<Box sx={{width: '100%', alignSelf: 'start'}}>
				<List>
					<MenuItem component={Link} to='/'>
						<Icon baseClassName='material-icons'>home</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							Home
						</Typography>
					</MenuItem>
					<Divider />
					<ListSubheader>Investments</ListSubheader>
					{investments.map((investment: Investment) => (
						<SidebarInvestment investment={investment} key={investment.investmentId} />
					))}
					<MenuItem onClick={() => setAddDialogOpen(true)}>
						<Icon baseClassName='material-icons'>add_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							Add Investment
						</Typography>
					</MenuItem>
					<Divider />
					<ListSubheader>Vehicles</ListSubheader>
					{vehicles.map((vehicle) => (
						<Box>
							<MenuItem
								component={Link}
								to={`/vehicle/${vehicle.investmentVehicleId}`}
								key={vehicle.investmentVehicleId}>
								<VehicleListItem
									vehicleName={vehicle.investmentVehicleName}
									vehicleType={vehicle.investmentVehicleType}
								/>
							</MenuItem>
							<Box sx={{marginLeft: '2rem'}}>
								{vehicle.investments.map((investment: Investment) => (
									<SidebarInvestment
										investment={investment}
										key={investment.investmentId}
									/>
								))}
								{/* TODO CHANGE DIALOG TARGET */}
								<MenuItem onClick={() => setAddDialogOpen(true)}>
									<Icon baseClassName='material-icons'>add_circle</Icon>
									<Typography
										variant='body1'
										component='div'
										sx={{marginLeft: '10px'}}>
										Add Investment to Vehicle
									</Typography>
								</MenuItem>
							</Box>
							<MenuItem onClick={() => setAddDialogOpen(true)}>
								<Icon baseClassName='material-icons'>add_circle</Icon>
								<Typography
									variant='body1'
									component='div'
									sx={{marginLeft: '10px'}}>
									Add Vehicle
								</Typography>
							</MenuItem>
						</Box>
					))}
					<Divider />
				</List>
			</Box>
		);
	};

	let contents = (
		<Paper elevation={2} sx={{marginX: '1rem', height: '90vh', width: '100%'}}>
			{renderInvestmentsTable(investments)}
		</Paper>
	);

	return (
		<div>
			<AppBar position='static' sx={{padding: '1rem'}}>
				<Box sx={{display: 'flex'}}>
					<Typography variant='h6' component='div'>
						RetireSimple
					</Typography>
					<Box component='span' sx={{flex: '1 1 auto'}} />
					<Tooltip title='Report Bug/Issue on GitHub'>
						<IconButton
							color='inherit'
							href='https://github.com/rhit-westeraj/RetireSimple/issues'>
							<Icon baseClassName='material-icons'>bug_report</Icon>
						</IconButton>
					</Tooltip>
				</Box>
			</AppBar>
			<Box sx={{marginTop: '0.5rem', display: 'flex', flexDirection: 'row'}}>
				<Box sx={{marginRight: '2rem'}}>{contents}</Box>
				<Box
					sx={{
						display: 'flex',
						marginY: '0.5rem',
						marginLeft: '1rem',
						maxWidth: '100vh',
					}}>
					<Outlet />
				</Box>
			</Box>
			<AddInvestmentDialog open={addDialogOpen} onClose={() => setAddDialogOpen(false)} />
		</div>
	);
};
