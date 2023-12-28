import {Box, Button, CircularProgress, Typography} from '@mui/material';
import React from 'react';
import {useNavigation} from 'react-router-dom';
import {getAggregateModel} from '../api/InvestmentApi';
import {PortfolioAggregateGraph, PortfolioBreakdownGraph} from '../components/GraphComponents';
import {convertPortfolioModelData, createAggregateStackData} from '../api/ApiMapper';
export const Root = () => {
	const [hasData, setHasData] = React.useState<boolean>(false);
	const [portfolioData, setPortfolioData] = React.useState<any[]>([]);
	const [breakdownData, setBreakdownData] = React.useState<any[]>([]);
	const [loadIndicator, setLoadIndicator] = React.useState<boolean>(true);
	const [noInvestments, setNoInvestments] = React.useState<boolean>(false);
	const navigation = useNavigation();

	

	React.useEffect(() => {
		if (navigation.state === 'loading') {
			setHasData(false);
		}
	}, [navigation.state]);

	getAggregateModel()
		.then((res) => {
			setPortfolioData(convertPortfolioModelData(res.portfolioModel));
			console.log(res.portfolioModel.portfolioModelId)
			if(res.portfolioModel.portfolioModelId === -1)
			{
				setNoInvestments(true);
				setHasData(false);
				setLoadIndicator(false);
			}
			else{
				setBreakdownData(createAggregateStackData(res.investmentModels));
				setHasData(true);
			}
		})
		.then(() => setLoadIndicator(false));

	return (
		
		<Box sx={{display: 'flex', flexDirection: 'column'}}>
			<Box
				sx={{
					display: 'flex',
					width: '95%',
					height: '50%',
					marginX: 'auto',
					flexDirection: 'column',
					objectFit: 'contain',
				}}>

				{loadIndicator && (
					<Box sx={{display: 'flex', alignItems: 'center'}}>
						<CircularProgress />
						<Typography variant='button' sx={{marginLeft: '0.25rem'}}>
							Generating Full Portfolio Model, this may take a minute...
						</Typography>
					</Box>
				)}
				{hasData && (
					<Box>
						<PortfolioAggregateGraph modelData={portfolioData} height={350} />
						<PortfolioBreakdownGraph modelData={breakdownData} height={350} />
					</Box>
				)}
				{noInvestments && (
					<Box>
						<h1>No Investment Data</h1>
					</Box>
				)}
			</Box>
		</Box>
	);
};
