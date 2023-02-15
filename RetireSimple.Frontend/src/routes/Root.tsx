import {Box, Button, Typography} from "@mui/material";
import React from "react";
import {useNavigation} from "react-router-dom";
import {CartesianGrid, Label, Legend, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";
import {getAggregateModel} from "../api/InvestmentApi";
import {convertPortfolioModelData} from "../data/ApiMapper";

export const Root = () => {
	const [hasData, setHasData] = React.useState<boolean>(false);
	const [portfolioData, setPortfolioData] = React.useState<any[]>([])
	const navigation = useNavigation();

	React.useEffect(() => {
		if (navigation.state === 'loading'){
			setHasData(false);
		}
	},
	[navigation.state],
	);

	const getData = () => {
		getAggregateModel().then((res)=>{
			setPortfolioData(convertPortfolioModelData(res.portfolioModel))
			setHasData(true);
		});
	}





	return (

		<Box sx={{ display: 'flex', flexDirection: 'column' }}>
			<Box sx={{ display: 'flex', width:'95%', height: '50%', marginX: 'auto', flexDirection:'column'}}>
				<Button onClick={getData} disabled={hasData} size='large'>Get Data</Button>
				{hasData &&
				<Box sx={{ display: 'flex', width:'100%', height: '50%', marginX: 'auto', flexDirection:'column'}}>
					<Typography variant='h6'>Portfolio Model</Typography>
					<ResponsiveContainer width='100%' height={'45%'} minHeight={400} minWidth={1200}>
						<LineChart data={portfolioData}>
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
				}
			</Box>
		</Box>


	)
};
