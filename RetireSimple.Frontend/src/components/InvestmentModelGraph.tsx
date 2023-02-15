import {Button} from '@mui/material';
import React from 'react';
import {useNavigation} from 'react-router-dom';
import {CartesianGrid, Label, Legend, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from 'recharts';
import {getInvestmentModel} from '../api/InvestmentApi';
import {convertInvestmentModelData} from '../data/ApiMapper';
import {InvestmentModel} from '../data/Interfaces';

export interface InvestmentModelGraphProps {
	investmentId: number;
}

export const InvestmentModelGraph = (props: InvestmentModelGraphProps) => {
	const [hasInitialData, setHasInitialData] = React.useState(false);
	const [modelData, setModelData] = React.useState<any[]>();

	const navigation = useNavigation();

	React.useEffect(() =>{
		if(navigation.state === 'loading') {
			setHasInitialData(false);
		}
	}
	,[navigation.state]);

	const getModelData = () => {
		getInvestmentModel(props.investmentId).then((data: InvestmentModel) => {
			setModelData(convertInvestmentModelData(data));
			setHasInitialData(true);
		});
	};

	return (
		<div>
			<Button onClick={getModelData} disabled={hasInitialData}>Get Model Data</Button>
			{hasInitialData ? (
				<ResponsiveContainer width='100%' height={400}>
					<LineChart data={modelData}>
						<XAxis dataKey='year'>
							<Label value='Months' offset={-5} position={"bottom"} />
						</XAxis>
						<YAxis tickCount={10} allowDecimals={false}
							type={'number'} tickFormatter={(value)=>value.toFixed(0)}>
							<Label value='$ Value (USD)' offset={0} position={"left"} angle={-90} />
						</YAxis>
						<CartesianGrid strokeDasharray='3 3' />
						<Tooltip />
						<Legend />
						<Line type='monotone' dataKey='min' stroke='#8884d8' />
						<Line type='monotone' dataKey='avg' stroke='#82ca9d' />
						<Line type='monotone' dataKey='max' stroke='#ff0000' />
					</LineChart>
				</ResponsiveContainer>)
				:<div></div>}
		</div>);
};
