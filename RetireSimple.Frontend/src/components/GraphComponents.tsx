import React from 'react';
import {getInvestmentModel} from '../api/InvestmentApi';
import {convertInvestmentModelData, convertVehicleModelData} from '../api/ApiMapper';
import {InvestmentModel} from '../Interfaces';
import {useNavigation} from 'react-router-dom';
import {
	Box,
	Button,
	CircularProgress,
	FormControlLabel,
	FormGroup,
	Switch,
	Typography,
} from '@mui/material';
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
import {getVehicleModel} from '../api/VehicleApi';

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

const loadIndicator = (
	<Box sx={{display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%'}}>
		<CircularProgress />
		<Typography variant='button' sx={{marginLeft: '0.25rem'}}>
			Generating Model...
		</Typography>
	</Box>
);

export const MinMaxAvgGraph = (props: {modelData: any[]}) => {
	return (
		<ResponsiveContainer width={300} height={200}>
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
				<CartesianGrid strokeDasharray='3 3' />
				<Tooltip />
				<Legend />
				<Line type='monotone' dataKey='min' stroke='#8884d8' />
				<Line type='monotone' dataKey='avg' stroke='#82ca9d' />
				<Line type='monotone' dataKey='max' stroke='#ff0000' />
			</LineChart>
		</ResponsiveContainer>
	);
};

export const InvestmentModelGraph = (props: {investmentId: number}) => {
	const [modelData, setModelData] = React.useState<any[] | undefined>(undefined);
	const [loading, setLoading] = React.useState<boolean>(false);

	const navigation = useNavigation();

	React.useEffect(() => {
		if (navigation.state === 'loading') {
			setModelData(undefined);
		}
	}, [navigation.state]);

	

	const getModelData = () => {
		setLoading(true);
		getInvestmentModel(props.investmentId)
			.then((data: InvestmentModel) => {
				setModelData(convertInvestmentModelData(data));
			})
			.then(() => setLoading(false));
	};

	if(!loading && modelData === undefined) {
		getModelData();
	}	

	return (
		<div>
						
			{/* {!loading && modelData === undefined && (
				<Box sx={{display: 'flex', justifyContent: 'center', alignItems: 'center'}}>
					<Button onClick={getModelData} disabled={modelData !== undefined}>
						Get Model Data
					</Button>
				</Box>
			)} */}
			{loading && loadIndicator}
			{modelData ? <MinMaxAvgGraph modelData={modelData} /> : <div></div>}
		</div>
	);
};

export const VehicleModelGraph = (props: {vehicleId: number}) => {
	const [modelData, setModelData] = React.useState<{base: any[]; taxed: any[]} | undefined>(
		undefined,
	);
	const [showTaxedModel, setShowTaxedModel] = React.useState<boolean>(false);
	const [loading, setLoading] = React.useState<boolean>(false);
	const navigation = useNavigation();

	React.useEffect(() => {
		if (navigation.state === 'loading') {
			setModelData(undefined);
		}
	}, [navigation.state]);

	const getModelData = () => {
		setLoading(true);
		getVehicleModel(props.vehicleId)
			.then((data: any) => {
				setModelData(convertVehicleModelData(data));
			})
			.then(() => setLoading(false));
		if(modelData) {
			console.log(modelData.taxed);

		}
	};

	return (
		<div>
			<Box>
				{!loading && modelData === undefined && (
					<Box sx={{display: 'flex', justifyContent: 'center', alignItems: 'center'}}>
						<Button onClick={getModelData} disabled={modelData !== undefined}>
							Get Model Data
						</Button>
					</Box>
				)}
				{loading && loadIndicator}
				<FormGroup>
					<FormControlLabel
						control={
							<Switch
								checked={showTaxedModel}
								onChange={() => setShowTaxedModel(!showTaxedModel)}
							/>
						}
						label='Show Tax-Applied Model'
					/>
				</FormGroup>
			</Box>
			{modelData ? (
				showTaxedModel ? (
					<>
						<Typography variant='h6'>Tax-Applied Model</Typography>
						<MinMaxAvgGraph modelData={modelData.taxed} />
					</>
				) : (
					<>
						<Typography variant='h6'>Base Model</Typography>
						<MinMaxAvgGraph modelData={modelData.base} />
					</>
				)
			) : (
				<div></div>
			)}
		</div>
	);
};

export const PortfolioBreakdownGraph = (props: {modelData: any[], height: number}) => {
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
			<ResponsiveContainer width='100%' minHeight={props.height} minWidth={1200}>
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

export const PortfolioAggregateGraph = (props: {modelData: any[], height: number}) => {
	return (
		<Box sx={{display: 'flex', flexDirection: 'column', width: '100%', height: '50%'}}>
			<Typography variant='h6'>Portfolio Model</Typography>
			<ResponsiveContainer width='100%'  minHeight={props.height} minWidth={1200}>
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
