import {Box, FormControl, FormHelperText, Grid, InputAdornment, InputLabel, MenuItem, Select, TextField, Typography} from '@mui/material';
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {AdapterDayjs} from '@mui/x-date-pickers/AdapterDayjs';

import dayjs, {Dayjs} from 'dayjs';
import React from 'react';
import {Controller, useFormContext, FormProvider} from 'react-hook-form';
import {MonteCarloAnalysisForm} from '../analysis/MonteCarloAnalysisForm';
import {StockTickerField} from './StockFields';

//Type aliases to shorten names
type ChangeEvent = React.ChangeEvent<HTMLInputElement>;
type StateSetter = React.Dispatch<React.SetStateAction<string>>;

export interface StockFormProps {
	defaultValues?: any;
}

export const StockForm = (props: StockFormProps) => {
	const formContext = useFormContext();
	//TODO Consolidate state, temp solution to ensure all components are controlled
	const [analysisType, setAnalysisType] = React.useState<string>(props.defaultValues?.analysisType ?? 'MonteCarlo_NormalDist');
	const [stockTicker, setStockTicker] = React.useState<string>(props.defaultValues?.stockTicker ?? '');
	const [stockPrice, setStockPrice] = React.useState<string>(props.defaultValues?.stockPrice ?? '');
	const [stockQuantity, setStockQuantity] = React.useState<string>(props.defaultValues?.stockQuantity ?? '');
	const [stockPurchaseDate, setStockPurchaseDate] = React.useState<string>(props.defaultValues?.stockPurchaseDate ?? '');
	const [stockDividendPercent, setStockDividendPercent] = React.useState<string>(props.defaultValues?.stockDividendPercent ?? '');
	const [stockDividendFirstPaymentDate, setStockDividendFirstPaymentDate] = React.useState<string>(props.defaultValues?.stockDividendFirstPaymentDate ?? '');

	const updateField = (setMethod: StateSetter, fieldName: string, value: string|null) => {
		setMethod(value ?? '');
		formContext.setValue(fieldName, value??'');
	};

	const onStockTickerChange = (event: ChangeEvent) => { updateField(setStockTicker, 'stockTicker', event.target.value);};
	const onStockPriceChange = (event: ChangeEvent) => { updateField(setStockPrice, 'stockPrice', event.target.value);};
	const onStockQuantityChange = (event: ChangeEvent) => { updateField(setStockQuantity, 'stockQuantity', event.target.value);};
	const onStockDividendPercentChange = (event:ChangeEvent) => { updateField(setStockDividendPercent, 'stockDividendPercent', event.target.value);};

	const {errors} = formContext.formState;

	React.useEffect(() => {
		formContext.reset({...props.defaultValues});
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

	// const reset = formContext.reset;

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
					value={stockTicker}
					onChange={onStockTickerChange}
					// defaultValue={props.defaultValues?.stockTicker ?? ''}
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
					value={stockPrice}
					onChange={onStockPriceChange}
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
					value={stockQuantity}
					onChange={onStockQuantityChange}
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
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<DatePicker
						{...field}
						label='Purchase Date'
						value={stockPurchaseDate}
						onChange={(date: Dayjs | null) => {
							setStockPurchaseDate(date?.format('YYYY-MM-DD') ?? dayjs().format('YYYY-MM-DD'));
							formContext.setValue('stockPurchaseDate', date?.format('YYYY-MM-DD') ?? dayjs().format('YYYY-MM-DD'));
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
					value={stockDividendPercent}
					onChange={onStockDividendPercentChange}
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
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<DatePicker
						{...field}
						label='First Payment Date'
						value={stockDividendFirstPaymentDate}
						onChange={(date: Dayjs | null) => {
							setStockDividendFirstPaymentDate(date?.format('YYYY-MM-DD') ?? dayjs().format('YYYY-MM-DD'));
							formContext.setValue('stockDividendFirstPaymentDate', date?.format('YYYY-MM-DD') ?? dayjs().format('YYYY-MM-DD'));
						}}
						renderInput={(params) =>
							<TextField {...params}
								size='small'
								error={!!errors.stockDividendFirstPaymentDate}
								helperText={errors.stockDividendFirstPaymentDate?.message as string}
							/>}
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
		<FormProvider {...formContext}>
			<Box sx={{flexGrow: 1, marginTop: '1rem'}}>
				<Typography variant='subtitle2'>Stock Information</Typography>
				<Grid container spacing={2}>
					<Grid item xs={2}>
						<StockTickerField control={formContext.control} default={props.defaultValues?.stockTicker ?? ''} errors={errors} />
					</Grid>
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
		</FormProvider>
	);
};
