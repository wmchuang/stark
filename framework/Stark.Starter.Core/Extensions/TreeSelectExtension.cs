namespace Stark.Starter.Core.Extensions;

public class TreeSelectExtension
{
    /// <summary>
    /// 递归公共方法
    /// </summary>
    /// <typeparam name="T">要递归的类</typeparam>
    /// <param name="roots">要递归的类集合</param>
    /// <returns>递归后的集合</returns>
    public static List<T> HandleTreeChildren<T>(List<T> roots) where T : GetTreeResult<T>
    {
        List<T> BuildTree(string parentId)
        {
            var returnList = new List<T>();
            var list = roots.Where(x => x.ParentId == parentId).OrderBy(x => x.Sort).ToList();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.Children = BuildTree(item.Id);
                    if (!item.Children.Any()) item.Children = null;
                    returnList.Add(item);
                }
            }
            return returnList;
        }

        var ids = roots.Select(x => x.Id).ToList();
        var parentIds = roots.Select(x => x.ParentId).ToList();
        //获取根节点ID（递归的起点），即父节点ID中不包含在ID中的节点，注意这里不能拿顶级节点 O 作为根节点，因为筛选的时候 O 会被过滤掉
        var rootIds = parentIds.Where(x => !ids.Contains(x)).Distinct().ToList();
        
        var returnList = new List<T>();
        
        foreach (var parentId in rootIds)
        {
            returnList.AddRange(BuildTree(parentId));
        }

        return returnList;
    }
}