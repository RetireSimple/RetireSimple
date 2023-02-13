import {Button} from '@mui/material';
import {CategoryScale, Chart, Legend, LineElement, LinearScale, PointElement, Title, Tooltip} from 'chart.js';
import React, {useMemo} from 'react';
import {Line} from 'react-chartjs-2';
import {useNavigation} from 'react-router-dom';
import {getInvestmentModel} from '../api/InvestmentApi';
import {InvestmentModel} from '../data/Interfaces';

export interface InvestmentModelGraphProps {
	investmentId: number;
}


export const InvestmentModelGraph = (props: InvestmentModelGraphProps) => {
	const [hasInitialData, setHasInitialData] = React.useState(false);
	const [modelData, setModelData] = React.useState<InvestmentModel>();
	const [dataLength, setDataLength] = React.useState<number>();

	const navigation = useNavigation();


	React.useEffect(() =>{
		if(navigation.state === 'loading') {
			setHasInitialData(false);
		}
	}
	,[navigation.state]);

	const data =
	React.useMemo(() =>
		modelData ?
			{
				labels: Array.from(new Array(dataLength), (x, i) => i),
				datasets: [
					{
						label: 'Min Model',
						data: modelData?.minModelData,
						borderColor: 'rgb(255, 99, 132)',
						backgroundColor: 'rgba(255, 99, 132, 0.5)',
					},
					{
						label: 'Avg Model',
						data: modelData?.avgModelData,
						borderColor: 'rgb(53, 162, 235)',
						backgroundColor: 'rgba(53, 162, 235, 0.5)',

					},
					{
						label: 'Max Model',
						data: modelData?.maxModelData,
						borderColor: 'rgb(53, 162, 52)',
						backgroundColor: 'rgba(53, 162, 52, 0.5)',
					},
				],
			}
			:
			{
				labels: Array.from(new Array(0), (x, i) => i),
				datasets: [],
			}
	,[modelData, dataLength]);

	const options = useMemo(() => ({
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
	}),[]);

	const getModelData = () => {
		getInvestmentModel(props.investmentId).then((data: InvestmentModel) => {
			setModelData(data);
			setDataLength(data.minModelData.length);
			setHasInitialData(true);
		});
	};

	Chart.register(
		CategoryScale,
		LinearScale,
		PointElement,
		LineElement,
		Title,
		Tooltip,
		Legend,
	);

	const chart = React.useMemo(()=>
		hasInitialData ?
			<Line options={options} data={data} />:
			<div></div>,
	[hasInitialData, options, data],
	)

	return (
		<div>
			<Button onClick={getModelData} disabled={hasInitialData}>Get Model Data</Button>
			{chart}
		</div>);
};
