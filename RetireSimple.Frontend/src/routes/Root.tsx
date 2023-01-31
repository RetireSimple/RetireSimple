import {AppBar, Box, Divider, Grid, Icon, MenuItem, Paper, Stack, Typography} from '@mui/material';
import Skeleton from '@mui/material/Skeleton';
import React from 'react';
import {getInvestments} from '../api/InvestmentApi';
import {InvestmentListItem, mapListItemProps} from '../components/Sidebar/InvestmentListItem';
import {AddInvestmentDialog} from '../dialogs/AddInvestmentDialog';
import {Investment} from '../models/Interfaces';
import {Link, Outlet} from 'react-router-dom';

export const Root = () => {
	const [investments, setInvestments] = React.useState<Investment[]>([]);
	const [addDialogOpen, setAddDialogOpen] = React.useState<boolean>(false);
	const [loading, setLoading] = React.useState<boolean>(true);

	const populateInvestmentData = async () => {
		setInvestments(await getInvestments());
		setLoading(false);
	};

	React.useEffect(() => {
		if (loading) {populateInvestmentData();}
	});

	const renderInvestmentsTable = (investments: Investment[]) => {
		return loading ?
			(<Skeleton variant='rectangular' width='100%' height={100} />)
			: (
				<Box sx={{outerHeight: '100%', width: '100%', alignSelf: 'start'}}>
					<Stack spacing={2}>
						{investments.map((investment: Investment) =>
							(
								<MenuItem component={Link} to={`/investment/${investment.investmentId}`}>
									<InvestmentListItem {...mapListItemProps(investment)} />
								</MenuItem>
							))}
						<Divider />
						<MenuItem onClick={() => setAddDialogOpen(true)}>
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
			<AddInvestmentDialog open={addDialogOpen} onClose={() => setAddDialogOpen(false)} />
		</div>

	);
};
