import {AppBar, Box, Divider, Icon, IconButton, List, ListSubheader, MenuItem, Paper, Tooltip, Typography} from '@mui/material';
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
				<Box sx={{display: 'flex'}}>
					<Typography variant='h6'
						component='div'>RetireSimple</Typography>
					<Box component='span' sx={{flex: '1 1 auto'}} />
					<Tooltip title='Report Bug/Issue on GitHub'>
						<IconButton color='inherit' href='https://github.com/rhit-westeraj/RetireSimple/issues'>
							<Icon baseClassName='material-icons'>bug_report</Icon>
						</IconButton>
					</Tooltip>
				</Box>
			</AppBar>
			<Box sx={{marginTop: '0.5rem', display: 'flex', flexDirection: 'row'}}>
				<Box sx={{marginRight: '2rem'}}>
					{contents}
				</Box>
				<Box sx={{display: 'flex', marginY: '0.5rem', marginLeft: '1rem', maxWidth: '100vh'}}>
					<Outlet />
				</Box>
			</Box>
			<AddInvestmentDialog open={addDialogOpen} onClose={() => setAddDialogOpen(false)} />
		</div>

	);
};