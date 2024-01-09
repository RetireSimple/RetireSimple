import React from 'react';
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
import { getTestData, convertVehicleModelData } from '../api/NewApiMapper';
import { Projection } from '../Interfaces';


export const AvgGraph = (props: {modelData: Projection}) => {

    

	return (
		<ResponsiveContainer width={300} height={200}>
			<LineChart data={props.modelData.values}>
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
				<Line type='monotone' dataKey='avg' stroke='#82ca9d' />
			</LineChart>
		</ResponsiveContainer>
	);
};
