type Error = {
  code: string
  description: string
}

export type Result<T> = {
  isSuccess: boolean;
  data: T
  errors: Array<Error>
}