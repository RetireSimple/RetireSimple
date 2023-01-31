import {Box, FormControl, FormHelperText, Grid, InputAdornment, InputLabel, MenuItem, Select, TextField, Typography} from '@mui/material';
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {AdapterMoment} from '@mui/x-date-pickers/AdapterMoment';

import React from 'react';
import {Controller, useFormContext} from 'react-hook-form';
import {MonteCarloAnalysisForm} from './analysis/MonteCarloAnalysisForm';

export const StockForm = () => {
	const formContext = useFormContext();
	const [analysisType, setAnalysisType] = React.useState<string>('MonteCarlo_NormalDist');

	const {errors} = formContext.formState;

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
			return <Typography variant='subtitle2'>No analysis parameters available</Typography>;
		}
	}, [analysisType]);

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const stockTickerField = (
		<Controller
			name='stockTicker'
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Ticker'
					fullWidth
					size='small'
					error={!!errors.stockTicker}
					helperText={errors.stockTicker?.message as string} />
			)} />);

	const stockPriceField = (
		<Controller
			name='stockPrice'
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Price'
					fullWidth
					size='small'
					error={!!errors.stockPrice}
					helperText={errors.stockPrice?.message as string}
					InputProps={{
						startAdornment: <InputAdornment position='start'>$</InputAdornment>,
					}} />
			)} />);

	const stockQuantityField = (
		<Controller
			name='stockQuantity'
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Quantity'
					fullWidth
					size='small'
					error={!!errors.stockQuantity}
					helperText={errors.stockQuantity?.message as string}
				/>
			)} />);

	const stockPurchaseDateField = (
		<Controller
			name='stockPurchaseDate'
			control={formContext.control}
			render={({field}) => (
				<LocalizationProvider dateAdapter={AdapterMoment}>
					<DatePicker
						label='Purchase Date'
						value={field.value}
						onChange={(newValue) => {
							formContext.setValue('stockPurchaseDate', newValue.toDate().toISOString('yyyy-MM-dd'));

						}}
						renderInput={(params) => <TextField {...params} size='small' />}
					/>
				</LocalizationProvider>
			)} />);

	const stockDividendPercentField = (
		<Controller
			name='stockDividendPercent'
			rules={{required: true, min: 0, max: 1}}
			control={formContext.control}
			render={({field}) => (
				<TextField {...field} label='Dividend Amt'
					fullWidth
					size='small'
					error={!!errors.stockDividendPercent}
					helperText={errors.stockDividendPercent?.message as string}
					InputProps={{
						endAdornment: <InputAdornment position='end'>%</InputAdornment>,
					}} />
			)} />);

	const stockDividendDistributionIntervalField = (
		<Controller
			name='stockDividendDistributionInterval'
			control={formContext.control}
			defaultValue={'Month'}
			render={({field}) => (
				<FormControl fullWidth
					error={!!errors.stockDividendDistributionInterval}
					size='small'
				>
					<InputLabel id='stockDividendDistributionInterval'>Dividend Int.</InputLabel>
					<Select {...field}
						value={field.value}
						label='Dividend Int.'>
						<MenuItem value='Month'>Monthly</MenuItem>
						<MenuItem value='Quarter'>Quarterly</MenuItem>
						<MenuItem value='Annual'>Annual</MenuItem>
					</Select>
					<FormHelperText>
						{errors.stockDividendDistributionInterval?.message as string}
					</FormHelperText>
				</FormControl>
			)} />);

	const stockDividendDistributionMethodField = (
		<Controller
			name='stockDividendDistributionMethod'
			control={formContext.control}
			defaultValue={'Stock'}
			render={({field}) => (
				<FormControl
					fullWidth
					error={!!errors.stockDividendDistributionMethod}
					size='small'
				>
					<InputLabel id='stockDividendDistributionMethod'>Dividend Method</InputLabel>
					<Select {...field}
						value={field.value}
						label='Dividend Method'>
						<MenuItem value='Stock'>Stock</MenuItem>
						<MenuItem value='Cash'>Cash</MenuItem>
						<MenuItem value='DRIP'>DRIP</MenuItem>
					</Select>
					<FormHelperText>
						{errors.stockDividendDistributionMethod?.message as string}
					</FormHelperText>
				</FormControl>
			)} />);

	const stockDividendFirstPaymentDateField = (
		<Controller
			name='stockDividendFirstPaymentDate'
			control={formContext.control}
			render={({field}) => (
				<LocalizationProvider dateAdapter={AdapterMoment}>
					<DatePicker
						label='First Payment Date'
						value={field.value}
						onChange={(newValue) => {
							formContext.setValue('stockDividentFirstPaymentDate', newValue);
						}}
						renderInput={(params) => <TextField {...params} size='small' />}
					/>
				</LocalizationProvider>
			)} />);

	const analysisTypeField = (
		<Controller
			name='analysisType'
			control={formContext.control}
			defaultValue={"MonteCarlo_NormalDist"}
			render={({field}) => (
				<FormControl fullWidth size='small'>
					<InputLabel id='analysisType'>Analysis Type</InputLabel>
					<Select {...field}
						value={field.value}
						onChange={e => {
							setAnalysisType(e.target.value);
							formContext.setValue('analysisType', e.target.value);
						}}
						label='Analysis Type'>
						<MenuItem value='testAnalysis'>Test Analysis</MenuItem>
						<MenuItem value='MonteCarlo_NormalDist'>Monte Carlo - Normal Dist.</MenuItem>
						<MenuItem value='MonteCarlo_LogNormalDist'>Monte Carlo - Log Normal Dist.</MenuItem>
					</Select>
				</FormControl>
			)} />);

	return (
		<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
			<Typography variant='subtitle2'>Stock Information</Typography>
			<Grid container spacing={2}>
				<Grid item xs={2}>{stockTickerField}</Grid>
				<Grid item xs={2}>{stockPriceField}</Grid>
				<Grid item xs={2}>{stockQuantityField}</Grid>
				<Grid item xs={4}>{stockPurchaseDateField}</Grid>
				<Grid item xs={2}></Grid>
				{/* Dividend Section */}
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Dividend Information</Typography>
				</Grid>
				<Grid item xs={2}>{stockDividendPercentField}</Grid>
				<Grid item xs={2}>{stockDividendDistributionIntervalField}</Grid>
				<Grid item xs={2}>{stockDividendDistributionMethodField}</Grid>
				<Grid item xs={4}>{stockDividendFirstPaymentDateField}</Grid>
				<Grid item xs={2}></Grid>
				{/* Analysis Section */}
				<Grid item xs={12}>
					<Typography variant='subtitle2'>Analysis Configuration</Typography>
				</Grid>
				<Grid item xs={4}>{analysisTypeField}</Grid>
				<Grid item xs={12}>
					{analysisSubForm()}
				</Grid>
			</Grid>
		</Box>
	);
};
