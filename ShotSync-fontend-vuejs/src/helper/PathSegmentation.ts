export const pathSegmentation = (path: string): string | undefined => {
  const pathSegments = path.split("/");
  return pathSegments[pathSegments.length - 1];
};
