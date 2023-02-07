import {TextField} from "@mui/material";
import React from "react";
import {Control, Controller, FieldErrorsImpl, FieldValues} from "react-hook-form";

export interface StockTickerFieldProps {
	default: string;
	control: Control<FieldValues>;
	errors: Partial<FieldErrorsImpl<{ [x: string]: any;}>>;	//RHF errors object
}

export const StockTickerField = (props: StockTickerFieldProps) => {
	const [stockTicker, setStockTicker] = React.useState<string>(props.default);

	const onStockTickerChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setStockTicker(event.target.value);
	};

	return (<>
		<Controller
			name='stockTicker'
			control={props.control}
			render={({field}) => (
				<TextField {...field}
					label='Ticker'
					fullWidth
					size='small'
					value={stockTicker}
					onChange={onStockTickerChange}
					// defaultValue={props.defaultValues?.stockTicker ?? ''}
					error={!!props.errors.stockTicker}
					helperText={props.errors.stockTicker?.message as string} />
			)} />
	</>);
}