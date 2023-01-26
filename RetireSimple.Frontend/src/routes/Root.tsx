import React from 'react';
import {useForm} from 'react-hook-form';
import {InvestmentModelGraph} from '../components/InvestmentModelGraph';
import {Investment, InvestmentModel, StockInfo} from '../models/Interfaces';
import {Box, Grid, Paper} from '@mui/material';
import {InvestmentListItem, mapListItemProps} from '../components/Sidebar/InvestmentListItem';
import Skeleton from '@mui/material/Skeleton';
import {addStock, getInvestmentModel, getInvestments} from '../api/InvestmentApi';

export const Root = () => {

	const [investments, setInvestments] = React.useState<Investment[]>([]);

	const [investmentModelId, setInvestmentModelId] = React.useState("");

	const [investmentModels, setInvestmentModels] = React.useState<InvestmentModel>();

	const [loading, setLoading] = React.useState<boolean>(true);

	const {register, handleSubmit} = useForm<StockInfo>();

	const onSubmit = handleSubmit((formData: any) => {
		addStock(formData)
			.then(async () => await populateInvestmentData());
	});

	const getAnalysis = async () => {
		setInvestmentModels(await getInvestmentModel(Number.parseInt(investmentModelId)));
	};

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
				<Box sx={{outerHeight: '100%', width: '20%', alignSelf: 'start'}}>
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

			{contents}

			<div>
				<form onSubmit={onSubmit}>
					<label>Ticker</label>
					<input {...register("ticker")} /><br />
					<label>Name</label>
					<input {...register("name")} /><br />
					<label>Price</label>
					<input {...register("price", {valueAsNumber: true})} /><br />
					<label>Quantity</label>
					<input {...register("quantity", {valueAsNumber: true})} /><br />
					<label>AnalysisType</label>
					<select {...register("analysisType")} >
						<option value="testAnalysis">Test Analysis</option>
						<option value="MonteCarlo_NormalDist">Monte Carlo - Normal Dist.</option>
						<option value="MonteCarlo_LogNormalDist">Monte Carlo - Log Normal Dist.</option>
					</select><br />
					<input type="submit" />
				</form>
			</div>

			<h1 id="tabelLabel" >Investments</h1>
			<p></p>
			<input type="text" onChange={e => setInvestmentModelId(e.target.value)}></input>
			<button onClick={getAnalysis}>Get Analysis Model</button>

			{chart}
		</div>

	);
};
