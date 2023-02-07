import {Box, FormControl, FormHelperText, Grid, InputLabel, MenuItem, Select, TextField} from '@mui/material';
import React from 'react';
import {Controller, FormProvider, useFormContext} from 'react-hook-form';
import {BondForm} from './investment/BondForm';
import {StockForm} from './investment/StockForm';

export interface InvestmentDataFormProps {
	defaultValues?: any;
}

///IMPORTANT CAVEAT: This form does not use a standard submit action
///Data should be validated by calling trigger, then true promise calls getValues()
///Allows for parents to retrieve data from the form context

export const InvestmentDataForm = (props: InvestmentDataFormProps) => {

	const [investmentType, setInvestmentType] = React.useState<string>("StockInvestment");
	const formContext = useFormContext();

	const {errors} = formContext.formState;

	const investmentTypeSubform = React.useCallback(() => {
		switch (investmentType) {
		case 'StockInvestment':
			return <StockForm />;
		case 'BondInvestment':
			return <BondForm />;
		default:
			return <div>Unknown investment type</div>;
		}
	}, [investmentType]);

	React.useEffect((
	) => {
		if(props.defaultValues){
			Object.keys(props.defaultValues).forEach((key: string) => {
				formContext.setValue(key, props.defaultValues?.value??'');
			});
		}
	// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [ props.defaultValues]);

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const investmentNameField = (
		<Controller
			name='investmentName'
			control={formContext.control}
			defaultValue=''
			render={({field}) => (
				<FormControl
					error={!!errors[`${field.name}`]}
					fullWidth
				>
					<TextField {...field} size='small' label='Investment Name' />
					<FormHelperText>{errors?.investmentName?.message as string}</FormHelperText>
				</FormControl>
			)} />);

	const investmentTypeField = (
		<Controller
			name='investmentType'
			control={formContext.control}
			defaultValue='StockInvestment'
			render={({field}) => (
				<FormControl
					error={!!errors.investmentType}
					fullWidth
					size='small'>
					<InputLabel id='investmentType'>Investment Type</InputLabel>
					<Select
						label='Investment Type'
						value={investmentType}
						onChange={(e) => {
							setInvestmentType(e.target.value as string);
							field.onChange(e);
						}}
						inputRef={formContext.register('investmentType').ref}
					>
						<MenuItem value='StockInvestment'>Stock</MenuItem>
						<MenuItem value='BondInvestment'>Bond</MenuItem>
					</Select>
					<FormHelperText>{errors?.investmentType?.message as string}</FormHelperText>
				</FormControl>
			)} />);

	return (<>

		<FormProvider {...formContext}>
			<Box>
				<Grid container spacing={2}>
					<Grid item xs={4}>{investmentNameField}</Grid>
					<Grid item xs={4}>{investmentTypeField}</Grid>
				</Grid>
				{investmentTypeSubform()}
			</Box>
		</FormProvider>
	</>);
};
