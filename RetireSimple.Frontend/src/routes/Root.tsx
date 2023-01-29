import {AppBar, Box, Grid, Paper, Typography} from '@mui/material';
import Skeleton from '@mui/material/Skeleton';
import React from 'react';
import {FieldValues} from 'react-hook-form';
import {getInvestments} from '../api/InvestmentApi';
import {InvestmentModelGraph} from '../components/InvestmentModelGraph';
import {InvestmentListItem, mapListItemProps} from '../components/Sidebar/InvestmentListItem';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {Investment, InvestmentModel} from '../models/Interfaces';

export const Root = () => {

	const [investments, setInvestments] = React.useState<Investment[]>([]);



	const [investmentModels, setInvestmentModels] = React.useState<InvestmentModel>();

	const [loading, setLoading] = React.useState<boolean>(true);


	// const getAnalysis = async () => {
	// 	setInvestmentModels(await getInvestmentModel(Number.parseInt(investmentModelId)));
	// };

	const populateInvestmentData = async () => {
		setInvestments(await getInvestments());
		setLoading(false);
	};

	React.useEffect(() => {
		if (loading) {populateInvestmentData();};
	});

	const renderAnalysis = () => {
		return (<InvestmentModelGraph
			modelData={investmentModels}
			dataLength={investmentModels?.avgModelData.length} />);
	};

	const renderInvestmentsTable = (investments: Investment[]) => {
		return loading ?
			(<Skeleton variant="rectangular" width="100%" height={100} />)
			: (
				<Box sx={{outerHeight: '100%', width: '100%', alignSelf: 'start'}}>
					<Grid container spacing={2}>
						{investments.map((investment: Investment) =>
						(<Grid item xs={12} key={investment.investmentId}>
							<InvestmentListItem {...mapListItemProps(investment)} />
						</Grid>))}
					</Grid>
				</Box>
			);
	};

	let chart = renderAnalysis();
	let contents = (<Paper elevation={2}>{renderInvestmentsTable(investments)}</Paper>);

	return (
		<div>
			<AppBar position="static" >
				<Typography variant="h6" component="div" sx={{flexGrow: 1}}>RetireSimple</Typography>
			</AppBar>
			<Grid container spacing={2} sx={{marginTop: '40px'}}>
				<Grid item xs={3}>
					{contents}
				</Grid>
				<Grid item xs={9}>

					<InvestmentDataForm onSubmit={(data: FieldValues) => {console.log(data);}} />

					{/*
					<input type="text" onChange={e => setInvestmentModelId(e.target.value)}></input>
					<button onClick={getAnalysis}>Get Analysis Model</button>

					{chart} */}
				</Grid>
			</Grid>
		</div>

	);
};
