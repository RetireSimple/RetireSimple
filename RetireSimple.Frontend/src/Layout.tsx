import {AppBar, Box, Divider, Icon, List, ListSubheader, MenuItem, Paper, Typography} from '@mui/material';
import React from 'react';
import {Link, Outlet, useLoaderData} from 'react-router-dom';
import {InvestmentListItem, mapListItemProps} from './components/Sidebar/InvestmentListItem';
import {AddInvestmentDialog} from './components/dialogs/AddInvestmentDialog';
import {Investment} from './data/Interfaces';

export const Layout = () => {
	const investments = useLoaderData() as Investment[];

	const [addDialogOpen, setAddDialogOpen] = React.useState(false);

	const renderInvestmentsTable = (investments: Investment[]) => {
		return (
			<Box sx={{width: '100%', alignSelf: 'start'}}>
				<List>
					<MenuItem component={Link} to='/'>
						<Icon baseClassName='material-icons'>home</Icon>
						<Typography variant='body1'
							component='div'
							sx={{marginLeft: '10px'}}>Home</Typography>
					</MenuItem>
					<Divider />
					<ListSubheader>Investments</ListSubheader>
					{investments.map((investment: Investment) => (
						<MenuItem component={Link}
							to={`/investment/${investment.investmentId}`}
							key={investment.investmentId}>
							<InvestmentListItem {...mapListItemProps(investment)} />
						</MenuItem>))}
					<Divider />
					<ListSubheader>Vehicles</ListSubheader>
					<MenuItem component={'div'}>
						Not Implemented Yet!
					</MenuItem>
					<Divider />
					<MenuItem onClick={() => setAddDialogOpen(true)}>
						<Icon baseClassName='material-icons'>add_circle</Icon>
						<Typography variant='body1'
							component='div'
							sx={{marginLeft: '10px'}}>Add Investment</Typography>
					</MenuItem>
				</List>
			</Box>
		);
	};

	let contents = (
		<Paper
			elevation={2}
			sx={{marginX: '1rem', height: '90vh', width: '100%'}}
		>{renderInvestmentsTable(investments)}</Paper>);

	return (
		<div>
			<AppBar position='static' sx={{padding: '1rem'}}>
				<Typography variant='h6'
					component='div' sx={{flexGrow: 1}}>RetireSimple</Typography>
			</AppBar>
			<Box sx={{marginTop: '0.5rem', display: 'flex', flexDirection: 'row', width: '100vh'}}>
				<Box sx={{marginRight: '2rem'}}>
					{contents}
				</Box>
				<Outlet />
			</Box>
			<AddInvestmentDialog open={addDialogOpen} onClose={() => setAddDialogOpen(false)} />
		</div>

	);
};