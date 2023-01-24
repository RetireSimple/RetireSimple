import React from 'react';
import { Line } from 'react-chartjs-2';
import { Investment, InvestmentModel, StockInfo } from './Models/Interfaces';
import { useForm } from "react-hook-form"
import {
	CategoryScale, Chart as ChartJS, Legend, LinearScale, LineElement, PointElement, Title,
	Tooltip
} from 'chart.js';


export default function App() {
	//const [forecasts, setForecasts] = React.useState<Forecast[]>([]);
	const [investments, setInvestments] = React.useState<Investment[]>([]);

	const [investmentModelId, setInvestmentModelId] = React.useState("");

	const [investmentModels, setInvestmentModels] = React.useState<InvestmentModel>();

	const [loading, setLoading] = React.useState<boolean>(true);

	const { register, setValue, handleSubmit, formState: { errors } } = useForm<StockInfo>();

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

	// const response = await fetch('/api/'); fetch data used in excel
	// const data = await response.json();
	ChartJS.register(
		CategoryScale,
		LinearScale,
		PointElement,
		LineElement,
		Title,
		Tooltip,
		Legend
	);

	//const labels = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

	const data = {
		//labels,
		labels: Array.from(new Array(60), (x, i) => i),
		datasets: [
			{
				label: 'Min Model',
				data: investmentModels?.minModelData,
				borderColor: 'rgb(255, 99, 132)',
				backgroundColor: 'rgba(255, 99, 132, 0.5)',
			},
			{
				label: 'Avg Model',
				data: investmentModels?.avgModelData,
				borderColor: 'rgb(53, 162, 235)',
				backgroundColor: 'rgba(53, 162, 235, 0.5)',

			},
			{
				label: 'Max Model',
				data: investmentModels?.maxModelData,
				borderColor: 'rgb(53, 162, 52)',
				backgroundColor: 'rgba(53, 162, 52, 0.5)',
			},
		],
	};
	const options = {
		responsive: true,
		plugins: {
			legend: {
				position: 'top' as const,
			},
			title: {
				display: true,
				text: 'Investment Model',
			},
		},
	};

	const getAnalysis = async () => {
		const response = await fetch(`/api/Analysis/GetAnaylsis?investmentID=${investmentModelId}`, { method: 'POST' });
		const data: InvestmentModel = await response.json();
		setInvestmentModels(data);
	}

	const populateInvestmentData = async () => {
		const response = await fetch('/api/Investment/GetAllInvestments');
		const data = await response.json();
		setInvestments(data);
		setLoading(false);
	};

	const addNewInvestment = async () => {
		const request = new Request('/api/Investment/AddRandomStock', { method: 'POST' })

		fetch(request).then(async () =>
			await populateInvestmentData());
	}

	React.useEffect(() => {
		if (loading) { populateInvestmentData() };
	});

	const renderAnalysis = (sim: String, options: any, data: any) => {
		switch (sim) {
			case "MonteCarlo":
				return <div> <Line options={options} data={data} /> </div>;
		}
	}

	const renderInvestmentsTable = (investments: Investment[]) => {
		return (
			<table className='table table-striped' aria-labelledby="tabelLabel">
				<thead>
					<tr>
						<th>ID</th>
						<th>Type</th>
						<th>Analysis Type</th>
						<th>Last Analysis</th>
						<th>Ticker</th>
						<th>Price</th>
						<th>Quantity</th>
						<th>Purchase Date</th>
					</tr>
				</thead>
				<tbody>
					{investments.map(inv =>
						<tr key={inv.investmentId}>
							<td>{inv.investmentId}</td>
							<td>{inv.investmentType}</td>
							<td>{inv.analysisType}</td>
							<td>{inv.lastAnalysis}</td>
							<td>{inv.investmentData['stockTicker']}</td>
							<td>{inv.investmentData['stockPrice']}</td>
							<td>{inv.investmentData['stockQuantity']}</td>
							<td>{inv.investmentData['stockPurchaseDate']}</td>
						</tr>
					)}
				</tbody>
			</table>
		);
	}

	let chart = renderAnalysis("MonteCarlo", options, data);

	let contents = loading
		? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
		: renderInvestmentsTable(investments);

	return (
		<div>
			<div>
				<form onSubmit={onSubmit}>
					<label>Ticker</label>
					<input {...register("ticker")} /><br />
					<label>Name</label>
					<input {...register("name")} /><br />
					<label>Price</label>
					<input {...register("price", { valueAsNumber: true })} /><br />
					<label>Quantity</label>
					<input {...register("quantity", { valueAsNumber: true })} /><br />
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

}
