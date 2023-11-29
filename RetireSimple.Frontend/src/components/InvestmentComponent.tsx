import { style  } from '@mui/system';
import {Box, Button, Divider, Tab, Tabs, Typography, Icon} from '@mui/material';

import {Investment} from '../Interfaces';
import { deleteInvestment } from '../api/InvestmentApi';

export const InvestmentComponent = (investment: Investment, callback: Function) => {
//deleteInvestment

	return <body onClick={() => callback()} style={{backgroundColor: 'gray', margin: '15px'}}>
		<div style={{width: '300px', paddingLeft: '10px', paddingBottom: '10px', paddingRight: '0px'}}>
			<span>
				<h2> {investment.investmentName} 
					<Button onClick={() => deleteInvestment(investment.investmentId)}>
						<Icon style={{color: 'black'}} baseClassName='material-icons'>edit_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							{/* Delete Investment */}
						</Typography>
					</Button>
					<Button onClick={() => deleteInvestment(investment.investmentId)}>
						<Icon style={{color: 'black'}} baseClassName='material-icons'>delete_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							{/* Delete Investment */}
						</Typography>
					</Button>
				</h2>				
			</span>
			<span>{investment.investmentName}</span>
			<br />
			<span>{investment.investmentData.stockTicker}</span>
			<br />
			<span>{investment.investmentData.stockQuantity}</span>			
		</div>
	</body>
}