import {Box, Button} from '@mui/material';
import React from 'react';
import {useNavigation} from 'react-router-dom';
import {getAggregateModel} from '../api/InvestmentApi';
import {PortfolioAggregateGraph, PortfolioBreakdownGraph} from '../components/GraphComponents';
import {convertPortfolioModelData, createAggregateStackData} from '../api/ApiMapper';
export const Root = () => {
	const [hasData, setHasData] = React.useState<boolean>(false);
	const [portfolioData, setPortfolioData] = React.useState<any[]>([]);
	const [breakdownData, setBreakdownData] = React.useState<any[]>([]);
	const navigation = useNavigation();

	React.useEffect(() => {
		if (navigation.state === 'loading') {
			setHasData(false);
		}
	}, [navigation.state]);

	const getData = () => {
		getAggregateModel().then((res) => {
			setPortfolioData(convertPortfolioModelData(res.portfolioModel));
			setBreakdownData(createAggregateStackData(res.investmentModels));
			setHasData(true);
		});
	};

	return (
		<Box sx={{display: 'flex', flexDirection: 'column'}}>
			<Box
				sx={{
					display: 'flex',
					width: '95%',
					height: '50%',
					marginX: 'auto',
					flexDirection: 'column',
				}}>
				{!hasData && (
					<Button onClick={getData} disabled={hasData} size='large'>
						Get Data
					</Button>
				)}
				{hasData && (
					<Box>
						<PortfolioAggregateGraph modelData={portfolioData} />
						<PortfolioBreakdownGraph modelData={breakdownData} />
					</Box>
				)}
			</Box>
		</Box>
	);
};
