import {Box, Typography} from '@mui/material';
import React from 'react';
import {Area, AreaChart, CartesianGrid, Label, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis} from 'recharts';

const strokeColors=['#8884d8', '#82ca9d', '#ff0000', '#ffc658', 'orange', 'pink', 'purple', 'brown']

export const PortfolioBreakdownGraph = (props: {modelData: any[]}) => {
	const breakdownAreas = React.useMemo(() => {
		if (props.modelData.length === 0) return (<></>);
		return Object.keys(props.modelData[0])
			.filter((key) => key !== 'month')
			.map((key, idx) => (
				<Area type='monotone'
					key={`area_${key}`}
					dataKey={key}
					stackId='1'
					stroke={strokeColors[idx]}
					fill={strokeColors[idx]} />
			));
	}, [props.modelData]);

	return (
		<Box sx={{ display: 'flex', width:'100%', height: '50%', marginX: 'auto', flexDirection:'column'}}>
			<Typography variant='h6'>Portfolio Breakdown (Average Model Projections)</Typography>
			<ResponsiveContainer width='100%' height={'45%'} minHeight={400} minWidth={1200}>
				<AreaChart data={props.modelData}>
					<XAxis dataKey='month'>
						<Label value='Months' offset={-5} position={"bottom"} />
					</XAxis>
					<YAxis tickCount={10} allowDecimals={false}
						type={'number'} tickFormatter={(value)=>value.toFixed(0)}>
						<Label value='$ Value (USD)' offset={0} position={"left"} angle={-90} />
					</YAxis>
					<CartesianGrid strokeDasharray='5 5' />
					<Tooltip />
					<Legend />
					{breakdownAreas}
				</AreaChart>
			</ResponsiveContainer>
		</Box>
	)
}
