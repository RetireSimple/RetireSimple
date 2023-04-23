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
	const [loadIndicator, setLoadIndicator] = React.useState<boolean>(false);
	const navigation = useNavigation();

	React.useEffect(() => {
		if (navigation.state === 'loading') {
			setHasData(false);
		}
	}, [navigation.state]);

	const getData = () => {
		setLoadIndicator(true);
		getAggregateModel()
			.then((res) => {
				setPortfolioData(convertPortfolioModelData(res.portfolioModel));
				setBreakdownData(createAggregateStackData(res.investmentModels));
				setHasData(true);
			})
			.then(() => setLoadIndicator(false));
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
					objectFit: 'contain',
				}}>
				{!hasData && (
					<>
						<Box>
							<Typography variant='h6' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
								Welcome to RetireSimple!
							</Typography>
							<Typography variant='body1' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
								To get started, add some information about investments or vehicles
								to the application.
							</Typography>
							<Typography variant='body1' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
								Once you have added some information, you can view a portfolio
								analysis by clicking the button below.
							</Typography>
							<Typography variant='body1' sx={{flexGrow: 1, marginBottom: '0.5rem'}}>
								You can come back here using the "Home" item in the top of the
								sidebar list.
							</Typography>
							<br />
							<Typography variant='h6'>DISCLAIMER</Typography>
							<Typography variant='body1'>
								<strong>
									The information provided by RetireSimple is for informational
									purposes only. It should not be considered as solid financial
									advice and should not be the only source of information you use
									to make financial decisions. You should consult with a certified
									financial professional (especially one that is a certified
									fiduciary) before making any financial decisions or taking any
									actions related to your finances.
								</strong>
							</Typography>
						</Box>
					</>
				)}

				{!loadIndicator && !hasData && (
					<Button onClick={getData} disabled={hasData} size='large' sx={{width: 'auto'}}>
						Get Portfolio Analysis
					</Button>
				)}
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
						<PortfolioAggregateGraph modelData={portfolioData} />
						<PortfolioBreakdownGraph modelData={breakdownData} />
					</Box>
				)}
			</Box>
		</Box>
	);
};
