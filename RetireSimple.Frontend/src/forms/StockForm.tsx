

import {Box, FormControl, Grid, InputAdornment, InputLabel, MenuItem, Select, TextField, Typography} from '@mui/material';
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {AdapterMoment} from '@mui/x-date-pickers/AdapterMoment';

import React from 'react';
import {Controller, useFormContext} from 'react-hook-form';
import {MonteCarloAnalysisForm} from './analysis/MonteCarloAnalysisForm';

export const StockForm = () => {
	const formContext = useFormContext();
	const [analysisType, setAnalysisType] = React.useState<string>('MonteCarlo_NormalDist');

	React.useEffect(() => {
		return () => {
			formContext.unregister('stockTicker');
			formContext.unregister('stockPrice');
			formContext.unregister('stockQuantity');
			formContext.unregister('stockPurchaseDate');
			formContext.unregister('stockDividendPercent');
			formContext.unregister('stockDividendDistributionInterval');
			formContext.unregister('stockDividendDistributionMethod');
			formContext.unregister('stockDividendFirstPaymentDate');
			formContext.unregister('analysisType');
		};
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	const analysisSubForm = React.useCallback(() => {
		switch (analysisType) {
			case 'MonteCarlo_NormalDist':
			case 'MonteCarlo_LogNormalDist':
				return <MonteCarloAnalysisForm />;
			default:
				return <Typography variant='subtitle2' >No analysis parameters available</Typography>;
		}
	}, [analysisType]);

	return (
		<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
			<Typography variant='subtitle2' >Stock Information</Typography>
			<Grid container spacing={2}>
				<Grid item xs={2}>
					<Controller
						name="stockTicker"
						control={formContext.control}
						render={({field}) => (
							<FormControl fullWidth>
								<TextField {...field} label='Ticker' />
							</FormControl>
						)}
					/>
				</Grid>
				<Grid item xs={2}>
					<Controller
						name="stockPrice"
						control={formContext.control}
						render={({field}) => (
							<TextField {...field} label='Price' fullWidth InputProps={{
								startAdornment: <InputAdornment position="start">$</InputAdornment>,
							}} />
						)}
					/>
				</Grid>

				<Grid item xs={2}>
					<Controller
						name="stockQuantity"
						control={formContext.control}
						render={({field}) => (
							<TextField {...field} label='Quantity' fullWidth />
						)}
					/>
				</Grid>
				<Grid item xs={4}>
					<Controller
						name="stockPurchaseDate"
						control={formContext.control}
						render={({field}) => (
							<LocalizationProvider dateAdapter={AdapterMoment}>
								<DatePicker
									label="Purchase Date"
									value={field.value}
									onChange={(newValue) => {
										formContext.setValue('stockPurchaseDate', newValue);
									}}
									renderInput={(params) => <TextField {...params} />}
								/>
							</LocalizationProvider>
						)} />
				</Grid>
				<Grid item xs={2}></Grid>
				{/* Dividend Section */}
				<Grid item xs={12}>
					<Typography variant='subtitle2' >Dividend Information</Typography>
				</Grid>
				<Grid item xs={2}>
					<Controller
						name="stockDividendPercent"
						rules={{required: true, min: 0, max: 1}}

						control={formContext.control}
						render={({field}) => (
							<TextField {...field} label='Dividend Amt' fullWidth InputProps={{
								endAdornment: <InputAdornment position="end">%</InputAdornment>,
							}} />
						)}
					/>
				</Grid>
				<Grid item xs={2}>
					<Controller
						name="stockDividendDistributionInterval"
						control={formContext.control}
						defaultValue={'Month'}
						render={({field}) => (
							<FormControl fullWidth>
								<InputLabel id="stockDividendDistributionInterval">Dividend Int.</InputLabel>
								<Select {...field}
									value={field.value}
									label='Divident Int.'>
									<MenuItem value="Month">Monthly</MenuItem>
									<MenuItem value="Quarter">Quarterly</MenuItem>
									<MenuItem value="Annual">Annual</MenuItem>
								</Select>
							</FormControl>
						)}
					/>
				</Grid>
				<Grid item xs={2}>
					<Controller
						name="stockDividendDistributionMethod"
						control={formContext.control}
						defaultValue={'Stock'}
						render={({field}) => (
							<FormControl fullWidth>
								<InputLabel id="stockDividendDistributionMethod">Dividend Method</InputLabel>
								<Select {...field}
									value={field.value}
									label='Dividend Method'>
									<MenuItem value="Stock">Stock</MenuItem>
									<MenuItem value="Cash">Cash</MenuItem>
									<MenuItem value="DRIP">DRIP</MenuItem>
								</Select>
							</FormControl>
						)}
					/>
				</Grid>
				<Grid item xs={4}>
					<Controller
						name="stockDividendFirstPaymentDate"
						control={formContext.control}
						render={({field}) => (
							<LocalizationProvider dateAdapter={AdapterMoment}>
								<DatePicker
									label="First Payment Date"
									value={field.value}
									onChange={(newValue) => {
										formContext.setValue('stockDividentFirstPaymentDate', newValue);
									}}
									renderInput={(params) => <TextField {...params} />}
								/>
							</LocalizationProvider>
						)} />
				</Grid>
				<Grid item xs={2}></Grid>
				{/* Analysis Section */}
				<Grid item xs={12}>
					<Typography variant='subtitle2' >Analysis Configuration</Typography>
				</Grid>
				<Grid item xs={4}>
					<Controller
						name="analysisType"
						control={formContext.control}
						defaultValue={"MonteCarlo_NormalDist"}
						render={({field}) => (
							<FormControl fullWidth>
								<InputLabel id="analysisType">Analysis Type</InputLabel>
								<Select {...field}
									value={field.value}
									onChange={e => {
										setAnalysisType(e.target.value);
										formContext.setValue('analysisType', e.target.value);
									}}
									label='Analysis Type'>
									<MenuItem value="testAnalysis">Test Analysis</MenuItem>
									<MenuItem value="MonteCarlo_NormalDist">Monte Carlo - Normal Dist.</MenuItem>
									<MenuItem value="MonteCarlo_LogNormalDist">Monte Carlo - Log Normal Dist.</MenuItem>
								</Select>
							</FormControl>
						)} />
				</Grid>
				<Grid item xs={12}>
					{analysisSubForm()}
				</Grid>
			</Grid>
		</Box >
	);
};
