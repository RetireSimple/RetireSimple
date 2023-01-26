import React from 'react';
import {InvestmentModel} from '../models/Interfaces';
import {CategoryScale, Chart, Legend, LineElement, LinearScale, PointElement, Title, Tooltip} from 'chart.js';
import {Line} from 'react-chartjs-2';

export interface InvestmentModelGraphProps {
	modelData?: InvestmentModel;
	dataLength?: number;
}

export const InvestmentModelGraph = (props: InvestmentModelGraphProps) => {

	const data = {
		labels: Array.from(new Array(props.dataLength), (x, i) => i),
		datasets: [
			{
				label: 'Min Model',
				data: props.modelData?.minModelData,
				borderColor: 'rgb(255, 99, 132)',
				backgroundColor: 'rgba(255, 99, 132, 0.5)',
			},
			{
				label: 'Avg Model',
				data: props.modelData?.avgModelData,
				borderColor: 'rgb(53, 162, 235)',
				backgroundColor: 'rgba(53, 162, 235, 0.5)',

			},
			{
				label: 'Max Model',
				data: props.modelData?.maxModelData,
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

	Chart.register(
		CategoryScale,
		LinearScale,
		PointElement,
		LineElement,
		Title,
		Tooltip,
		Legend
	);

	return (
		<div>
			<Line options={options} data={data} />
		</div>);
};
