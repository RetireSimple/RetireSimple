import React from 'react';
import {getInvestmentModel} from '../api/InvestmentApi';
import {convertInvestmentModelData} from '../api/ApiMapper';
import {InvestmentModel} from '../Interfaces';
import {useNavigation} from 'react-router-dom';
import {Box, Button, Typography} from '@mui/material';
import {
	Area,
	AreaChart,
	CartesianGrid,
	Label,
	Legend,
	Line,
	LineChart,
	ResponsiveContainer,
	Tooltip,
	XAxis,
	YAxis,
} from 'recharts';

const strokeColors = [
	'#8884d8',
	'#82ca9d',
	'#ff0000',
	'#ffc658',
	'orange',
	'pink',
	'purple',
	'brown',
];

export const InvestmentModelGraph = (props: {investmentId: number}) => {
	const [hasInitialData, setHasInitialData] = React.useState(false);
	const [modelData, setModelData] = React.useState<any[]>();

	const navigation = useNavigation();

	React.useEffect(() => {
		if (navigation.state === 'loading') {
			setHasInitialData(false);
		}
	}, [navigation.state]);

	const getModelData = () => {
		getInvestmentModel(props.investmentId).then((data: InvestmentModel) => {
			setModelData(convertInvestmentModelData(data));
			setHasInitialData(true);
		});
	};

	return (
		<div>
			<Button onClick={getModelData} disabled={hasInitialData}>
				Get Model Data
			</Button>
			{hasInitialData ? (
				<ResponsiveContainer width='100%' height={400}>
					<LineChart data={modelData}>
						<XAxis dataKey='year'>
							<Label value='Months' offset={-5} position={'bottom'} />
						</XAxis>
						<YAxis
							tickCount={10}
							allowDecimals={false}
							type={'number'}
							tickFormatter={(value) => value.toFixed(0)}>
							<Label value='$ Value (USD)' offset={0} position={'left'} angle={-90} />
						</YAxis>
						<CartesianGrid strokeDasharray='3 3' />
						<Tooltip />
						<Legend />
						<Line type='monotone' dataKey='min' stroke='#8884d8' />
						<Line type='monotone' dataKey='avg' stroke='#82ca9d' />
						<Line type='monotone' dataKey='max' stroke='#ff0000' />
					</LineChart>
				</ResponsiveContainer>
			) : (
				<div></div>
			)}
		</div>
	);
};

export const PortfolioBreakdownGraph = (props: {modelData: any[]}) => {
	const breakdownAreas = React.useMemo(() => {
		if (props.modelData.length === 0) return <></>;
		return Object.keys(props.modelData[0])
			.filter((key) => key !== 'month')
			.map((key, idx) => (
				<Area
					type='monotone'
					key={`area_${key}`}
					dataKey={key}
					stackId='1'
					stroke={strokeColors[idx]}
					fill={strokeColors[idx]}
				/>
			));
	}, [props.modelData]);

	return (
		<Box
			sx={{
				display: 'flex',
				width: '100%',
				height: '50%',
				marginX: 'auto',
				flexDirection: 'column',
			}}>
			<Typography variant='h6'>Portfolio Breakdown (Average Model Projections)</Typography>
			<ResponsiveContainer width='100%' height={'45%'} minHeight={400} minWidth={1200}>
				<AreaChart data={props.modelData}>
					<XAxis dataKey='month'>
						<Label value='Months' offset={-5} position={'bottom'} />
					</XAxis>
					<YAxis
						tickCount={10}
						allowDecimals={false}
						type={'number'}
						tickFormatter={(value) => value.toFixed(0)}>
						<Label value='$ Value (USD)' offset={0} position={'left'} angle={-90} />
					</YAxis>
					<CartesianGrid strokeDasharray='5 5' />
					<Tooltip />
					<Legend />
					{breakdownAreas}
				</AreaChart>
			</ResponsiveContainer>
		</Box>
	);
};

export const PortfolioAggregateGraph = (props: {modelData: any[]}) => {
	return (
		<Box sx={{display: 'flex', flexDirection: 'column', width: '100%', height: '50%'}}>
			<Typography variant='h6'>Portfolio Model</Typography>
			<ResponsiveContainer width='100%' height={'45%'} minHeight={400} minWidth={1200}>
				<LineChart data={props.modelData}>
					<XAxis dataKey='year'>
						<Label value='Months' offset={-5} position={'bottom'} />
					</XAxis>
					<YAxis
						tickCount={10}
						allowDecimals={false}
						type={'number'}
						tickFormatter={(value) => value.toFixed(0)}>
						<Label value='$ Value (USD)' offset={0} position={'left'} angle={-90} />
					</YAxis>
					<CartesianGrid strokeDasharray='5 5' />
					<Tooltip />
					<Legend />
					<Line type='monotone' dataKey='min' stroke='#8884d8' />
					<Line type='monotone' dataKey='avg' stroke='#82ca9d' />
					<Line type='monotone' dataKey='max' stroke='#ff0000' />
				</LineChart>
			</ResponsiveContainer>
		</Box>
	);
};
