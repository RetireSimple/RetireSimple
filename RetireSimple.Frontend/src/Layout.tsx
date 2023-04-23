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
import {ApiPresetData, Investment, Portfolio} from './Interfaces';
import {SidebarInvestment, VehicleListItem} from './components/SidebarComponents';
import {AddInvestmentDialog, AddVehicleDialog} from './components/DialogComponents';

import {getAnalysisPresets} from './api/ApiCommon';
import {AboutDialog} from './components/AboutDialog';

export const PresetContext = React.createContext<ApiPresetData | undefined>(undefined);

export const Layout = () => {
	const portfolio = useLoaderData() as Portfolio;
	const {investments, investmentVehicles: vehicles} = portfolio;

	const [presetData, setPresetData] = React.useState<ApiPresetData | undefined>(undefined);
	const [invAddDialogOpen, setInvAddDialogOpen] = React.useState(false);
	const [vehicleAddDialogOpen, setVehicleAddDialogOpen] = React.useState(false);
	const [vehicleAddInvTarget, setVehicleAddInvTarget] = React.useState<number>(-1); //by default, adds as individual investment
	const [aboutOpen, setAboutOpen] = React.useState(false);

	React.useEffect(() => {
		if (presetData === undefined) {
			getAnalysisPresets().then((data) => {
				setPresetData(data);
			});
		}
	}, [presetData]);

	const openAddInvDialog = (vehicleId: number) => {
		setVehicleAddInvTarget(vehicleId);
		setInvAddDialogOpen(true);
	};

	const renderInvestmentsTable = (
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
				<MenuItem onClick={() => openAddInvDialog(-1)}>
					<Icon baseClassName='material-icons'>add_circle</Icon>
					<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
						Add Investment
					</Typography>
				</MenuItem>
				<Divider />
				<ListSubheader>Vehicles</ListSubheader>
				{vehicles.map((vehicle) => (
					<Box key={vehicle.investmentVehicleId}>
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
							<MenuItem onClick={() => openAddInvDialog(vehicle.investmentVehicleId)}>
								<Icon baseClassName='material-icons'>add_circle</Icon>
								<Typography
									variant='body1'
									component='div'
									sx={{marginLeft: '10px'}}>
									Add Investment to Vehicle
								</Typography>
							</MenuItem>
						</Box>
					</Box>
				))}
				<MenuItem onClick={() => setVehicleAddDialogOpen(true)}>
					<Icon baseClassName='material-icons'>add_circle</Icon>
					<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
						Add Vehicle
					</Typography>
				</MenuItem>
				<Divider />
			</List>
		</Box>
	);

	let contents = (
		<Paper
			elevation={2}
			sx={{marginX: '1rem', height: '90vh', width: '100%', overflow: 'auto'}}>
			{renderInvestmentsTable}
		</Paper>
	);

	return (
		<div>
			<PresetContext.Provider value={presetData}>
				<AppBar position='static' sx={{padding: '1rem'}}>
					<Box sx={{display: 'flex'}}>
						<Typography variant='h6' component='div'>
							RetireSimple
						</Typography>
						<Box component='span' sx={{flex: '1 1 auto'}} />
						<Tooltip title='About'>
							<IconButton color='inherit' onClick={() => setAboutOpen(true)}>
								<Icon baseClassName='material-icons'>info</Icon>
							</IconButton>
						</Tooltip>
						<Tooltip title='Report Bug/Issue on GitHub'>
							<IconButton
								color='inherit'
								href='https://github.com/RetireSimple/RetireSimple/issues/new/choose'>
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
				<AddInvestmentDialog
					open={invAddDialogOpen}
					onClose={() => setInvAddDialogOpen(false)}
					vehicleTarget={vehicleAddInvTarget}
				/>
				<AddVehicleDialog
					open={vehicleAddDialogOpen}
					onClose={() => setVehicleAddDialogOpen(false)}
				/>
				<AboutDialog open={aboutOpen} onClose={() => setAboutOpen(false)} />
			</PresetContext.Provider>
		</div>
	);
};
