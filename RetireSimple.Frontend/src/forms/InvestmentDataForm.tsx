import {Box, FormControl, Grid, InputLabel, MenuItem, Select, TextField} from '@mui/material';
import React from 'react';
import {Controller, FormProvider, useForm} from 'react-hook-form';
import {StockForm} from './StockForm';
import {BondForm} from './BondForm';

export const InvestmentDataForm = ({onSubmit}: {onSubmit: any;}) => {

	const [investmentType, setInvestmentType] = React.useState<string>("StockInvestment");

	const formContext = useForm();

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

	return (
		<>
			<FormProvider {...formContext}>
				<form onSubmit={formContext.handleSubmit(onSubmit)}>
					<Box sx={{margin: '2rem'}}>
						<Grid container spacing={2}>
							<Grid item xs={6}>
								<Controller
									name='investmentName'
									control={formContext.control}
									defaultValue=''
									render={({field}) => (
										<FormControl fullWidth>
											<TextField {...field} label='Investment Name' />
										</FormControl>
									)}
								/>
							</Grid>
							<Grid item xs={6}>
								<Controller
									name='investmentType'
									control={formContext.control}
									defaultValue={'StockInvestment'}

									render={({field}) => (
										<FormControl fullWidth>
											<InputLabel id='investmentType'>Investment Type</InputLabel>
											<Select
												{...field}
												label='Investment Type'
												value={field.value}
												onChange={e => {
													setInvestmentType(e.target.value);
													formContext.setValue('investmentType', e.target.value);
												}}
											>
												<MenuItem value='StockInvestment'>Stock</MenuItem>
												<MenuItem value='BondInvestment'>Bond</MenuItem>
											</Select>
										</FormControl>
									)}
								/>
							</Grid>
							{investmentTypeSubform()}
							<input type="submit" />
						</Grid>
					</Box>
				</form>
			</FormProvider>

		</>
	);
};
