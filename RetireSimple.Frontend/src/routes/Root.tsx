import React from 'react';
import {useForm} from 'react-hook-form';
import {InvestmentModelGraph} from '../components/InvestmentModelGraph';
import {Investment, InvestmentModel, StockInfo} from '../models/Interfaces';
import {Box, Grid} from '@mui/material';
import {InvestmentListItem, mapListItemProps} from '../components/Sidebar/InvestmentListItem';
import Skeleton from '@mui/material/Skeleton';

export const Root = () => {

	const [investments, setInvestments] = React.useState<Investment[]>([]);

	const [investmentModelId, setInvestmentModelId] = React.useState("");

	const [investmentModels, setInvestmentModels] = React.useState<InvestmentModel>();

	const [loading, setLoading] = React.useState<boolean>(true);

	const {register, handleSubmit} = useForm<StockInfo>();

	const onSubmit = handleSubmit((formData: any) => {
		fetch('/api/Investment/AddStock', {
			method: 'POST', body: JSON.stringify(formData),
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json; charset=utf-8'
			},
		})
			.then(async () => await populateInvestmentData());
	});

	const getAnalysis = async () => {
		const response = await fetch(`/api/Analysis/GetAnaylsis?investmentID=${investmentModelId}`, {method: 'POST'});
		const data: InvestmentModel = await response.json();
		setInvestmentModels(data);
	};

	const populateInvestmentData = async () => {
		const response = await fetch('/api/Investment/GetAllInvestments');
		const data = await response.json();
		setInvestments(data);
		setLoading(false);
	};

	const addNewInvestment = async () => {
		const request = new Request('/api/Investment/AddRandomStock', {method: 'POST'});

		fetch(request).then(async () =>
			await populateInvestmentData());
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
	let contents = renderInvestmentsTable(investments);

	return (
		<div>
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

			<button onClick={addNewInvestment}>Add New Random Stonk</button>

			<h1 id="tabelLabel" >Investments</h1>
			<p></p>
			<input type="text" onChange={e => setInvestmentModelId(e.target.value)}></input>
			<button onClick={getAnalysis}>Get Analysis Model</button>

			{chart}
			{contents}
		</div>

	);
};
