import {AppBar, Box, Divider, Grid, Icon, MenuItem, Paper, Stack, Typography} from '@mui/material';
import {Link, Outlet, useLoaderData, useLocation} from 'react-router-dom';
import {InvestmentListItem, mapListItemProps} from './components/Sidebar/InvestmentListItem';
import {Investment} from './data/Interfaces';

export const App = () => {


	return (
		<div>

		</div>
	)
}

export const Layout = () =>{
	const location = useLocation();
	const investments = useLoaderData() as Investment[];
	// const [addDialogOpen, setAddDialogOpen] = React.useState<boolean>(false);

	const renderInvestmentsTable = (investments: Investment[]) => {
		return	(
			<Box sx={{outerHeight: '100%', width: '100%', alignSelf: 'start'}}>
				<Stack spacing={2}>
					{investments.map((investment: Investment) =>
						(
							<MenuItem component={Link} to={`/investment/${investment.investmentId}`}>
								<InvestmentListItem {...mapListItemProps(investment)} />
							</MenuItem>
						))}
					<Divider />
					<MenuItem component={Link} to={'investment/add'} state={({backgroundLocation:location})}>
						<Icon baseClassName='material-icons'>add_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>Add Investment</Typography>
					</MenuItem>
				</Stack>
			</Box>
		);
	};

	let contents = (<Paper elevation={2}>{renderInvestmentsTable(investments)}</Paper>);

	return (
		<div>
			<AppBar position='static'>
				<Typography variant='h6' component='div' sx={{flexGrow: 1}}>RetireSimple</Typography>
			</AppBar>
			<Grid container spacing={2} sx={{marginTop: '40px'}}>
				<Grid item xs={3}>
					{contents}
				</Grid>
				<Grid item xs={9}>
					<Outlet />
				</Grid>
			</Grid>
		</div>

	);
}