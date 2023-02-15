import {Box, Typography} from '@mui/material';
import {CartesianGrid, Label, Legend, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from 'recharts';

export const PortfolioAggregateGraph = (props: {modelData: any[]}) => {
	return (
		<Box sx={{display: 'flex', flexDirection: 'column', width: '100%', height: '50%'}}>
			<Typography variant='h6'>Portfolio Model</Typography>
			<ResponsiveContainer width='100%' height={'45%'} minHeight={400} minWidth={1200}>
				<LineChart data={props.modelData}>
					<XAxis dataKey='year'>
						<Label value='Months' offset={-5} position={"bottom"} />
					</XAxis>
					<YAxis tickCount={10} allowDecimals={false}
						type={'number'} tickFormatter={(value)=>value.toFixed(0)}>
						<Label value='$ Value (USD)' offset={0} position={"left"} angle={-90} />
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
	)
}
