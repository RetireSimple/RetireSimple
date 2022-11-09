export interface Investment {
    investmentId: number;
    investmentType: string;
    analysisType?: string;
    lastAnalysis?: string; //treat as date later?
    investmentData: { [key: string]: string }
}

export interface InvestmentModel {
    investmentModelId: number;
    investmentId: number;
    lastUpdated: string;    //treat as a date later?
    maxModelData: { item1: number, item2: number }[]

}